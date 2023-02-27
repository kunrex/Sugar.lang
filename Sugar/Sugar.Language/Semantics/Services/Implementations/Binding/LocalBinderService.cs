using System;
using System.Collections.Generic;

using Sugar.Language.Tokens.Enums;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Types;
using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Describers;
using Sugar.Language.Parsing.Nodes.Statements;
using Sugar.Language.Parsing.Nodes.Types.Enums;
using Sugar.Language.Parsing.Nodes.Expressions;
using Sugar.Language.Parsing.Nodes.Functions.Calling;
using Sugar.Language.Parsing.Nodes.Expressions.Operative;
using Sugar.Language.Parsing.Nodes.Functions.Declarations;
using Sugar.Language.Parsing.Nodes.Expressions.Associative;
using Sugar.Language.Parsing.Nodes.Statements.VariableCreation;
using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;

using Sugar.Language.Semantics.Analysis;
using Sugar.Language.Semantics.ActionTrees;
using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Namespaces;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.Services.Interfaces.Binding;
using Sugar.Language.Semantics.ActionTrees.CreationStatements;
using Sugar.Language.Parsing.Nodes.Functions.Calling.Structure;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Local;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.PropertyCreation;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation.Local;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation.Local.FunctionArguments;

using Sugar.Language.Exceptions.Analytics.Referencing;
using Sugar.Language.Semantics.ActionTrees.Interfaces;
using Sugar.Language.Exceptions.Analytics.ClassMemberCreation;
using Sugar.Language.Semantics.Services.Implementations.Structures;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes.Casts;
using Sugar.Language.Exceptions.Analytics.ClassMemberCreation.Statements;
using Sugar.Language.Exceptions.Analytics.ClassMemberCreation.SubTypeSearching;

namespace Sugar.Language.Semantics.Services.Implementations.Binding
{
    internal sealed class LocalBinderService : BinderService<ILocalBinderService>
    {
        protected override string StepName { get => "Local Binding"; }

        public LocalBinderService(DefaultNameSpaceNode _defaultNameSpace, CreatedNameSpaceCollectionNode _createdNameSpaces) : base(_defaultNameSpace, _createdNameSpaces)
        {
            
        }

        public override SemanticsResult Validate()
        {
            var integer = defaultNameSpace.GetInternalDataType(Analysis.BuiltInTypes.Enums.TypeEnum.Integer);
            Validate((StructType)integer);

            return result.Build();
        }

        private void Validate(DataType dataType)
        {
            var searcherService = new TypeSearcherService(dataType, defaultNameSpace, createdNameSpaces);

            var properties = dataType.GetAllMembers(MemberEnum.Properties);
            var functions = dataType.GetAllMembers(MemberEnum.Void | MemberEnum.Function);

            CreateFunctions(functions, searcherService);
            ProcessProperties(properties, searcherService, CreateLocalMembers);

            foreach(var function in functions)
                BindScope(((IFunction)function).Scope, searcherService);

            ProcessProperties(properties, searcherService, BindScope);
        }

        private void CreateFunctions(IEnumerable<ICreationStatement> functions, TypeSearcherService searcherService)
        {
            foreach (var function in functions)
            {
                var converted = (IFunction)function;

                var body = converted.Scope.Body;
                if (body.NodeType == NodeType.Scope)
                    CreateLocalMembers(converted.Scope, searcherService);
                else
                {
                    //probably throw a warning
                }
            }
        }

        private void ProcessProperties(IEnumerable<ICreationStatement> properties, TypeSearcherService searcherService, Action<Scope, TypeSearcherService> action)
        {
            foreach (var property in properties)
            {
                var declaration = (IProperty)property;

                switch (declaration.PropertyType)
                {
                    case PropertyTypeEnum.Get:
                        action.Invoke(declaration.GetExpression.Scope, searcherService);
                        break;
                    case PropertyTypeEnum.Set:
                        action.Invoke(declaration.SetExpression.Scope, searcherService);
                        break;
                    case PropertyTypeEnum.GetSet:
                        action.Invoke(declaration.GetExpression.Scope, searcherService);
                        action.Invoke(declaration.SetExpression.Scope, searcherService);
                        break;
                }
            }
        }

        private void BindScope(Scope scope, TypeSearcherService searcherService)
        {
            var body = scope.Body;

            if (body.NodeType == NodeType.Scope)
                foreach (var child in body.GetChildren())
                    switch (child.NodeType)
                    {
                        case NodeType.Initialise:
                        case NodeType.Declaration:
                            CreatLocalVariable((DeclarationNode)child, scope, searcherService);
                            break;
                        case NodeType.Assignment:
                            var assignment = (AssignmentNode)child;

                            IReturnableCreationStatement variable = ReferenceEntity(assignment.Value, scope, searcherService);

                            if(variable.CheckDescription(DescriberEnum.MutabilityModifier))
                            {
                                //exception
                                break;
                            }

                            var evaluatedType = ResolveEvaluable(assignment.To, scope, searcherService);
                            if (variable.CreationType != evaluatedType)
                            {
                                //exception
                            }


                            break;
                        case NodeType.FunctionCall:
                            ReferenceInClassFunction((FunctionCallNode)child, scope, searcherService);
                            break;
                    }
        }

        private void CreateLocalMembers(Scope parent, TypeSearcherService searcherService)
        {
            var body = parent.Body;

            if (body.NodeType == NodeType.Scope)
                foreach (var child in body.GetChildren())
                    switch (child.NodeType)
                    {
                        case NodeType.Scope:
                            var scope = new Scope(child);
                            CreateLocalMembers(scope, searcherService);

                            parent.AddScope(scope);
                            break;
                        case NodeType.MethodDeclaration:
                            CreateLocalFunction((FunctionDeclarationNode)child, parent, searcherService);
                            break;
                    }
        }

        private void CreatLocalVariable(DeclarationNode declaration, Scope scope, TypeSearcherService searcherService)
        {
            var name = (IdentifierNode)declaration.Name;
            if(scope.TryFindVariableCreation(name) != null)
            {
                result.Add(new DuplicateGlobalDefinitionException(name.Value));
                return;
            }

            var type = FindReferencedType(searcherService, (TypeNode)declaration.Type);
            if (type == null)
                return;

            var describer = describerService.AnalyseDescriber((DescriberNode)declaration.Describer);
            switch(declaration.NodeType)
            {
                case NodeType.Declaration:
                    scope.AddDeclaration(new LocalVariableDeclarationStmt(type, name, describer));
                    break;
                case NodeType.Initialise:
                    var expression = ((InitializeNode)declaration).Value;
                    var evaluated = ResolveEvaluable(expression, scope, searcherService);

                    if(evaluated != type)
                    {
                        //exception
                        return;
                    }

                    scope.AddDeclaration(new LocalVariableInitialisationStmt(type, name, describer, expression));
                    break;
            }
        }

        private void CreateLocalFunction(FunctionDeclarationNode function, Scope parent, TypeSearcherService subTypeSearcher)
        {
            var name = (IdentifierNode)function.Name;
            var arguments = CreateFunctionArguments(subTypeSearcher, parent, (FunctionDeclarationArgumentsNode)function.Arguments);

            if (parent.TryFindMethodDeclaration(name, arguments) == null)
            {
                if(parent.TryFindVoidDeclaration(name, arguments) != null)
                {
                    result.Add(new DuplicateGlobalDefinitionException(name.Value));
                    return;
                }
            }
            else
            {
                result.Add(new DuplicateGlobalDefinitionException(name.Value));
                return;
            }

            var type = (TypeNode)function.Type;
            var describer = new Describer(DescriberEnum.None);
  
            IFunction localFunction;
            if (type.Type == TypeNodeEnum.Void)
            {
                var localVoid = new LocalVoidDeclarationStmt(name, describer, arguments, function.Body);
                parent.AddDeclaration(localVoid);

                localFunction = localVoid;
            }
            else
            {
                var referenced = FindReferencedType(subTypeSearcher, type);
                if (type == null)
                    return;

                var localMethod = new LocalMethodCreationStmt(referenced, name, describer, arguments, function.Body);
                parent.AddDeclaration(localMethod);

                localFunction = localMethod;
            }

            CreateLocalMembers(localFunction.Scope, subTypeSearcher);
        }

        //you're gonna have to figure this out, like prop prop
        //probably the most logical thing yu have to do and thereofre the most fun :D
        private DataType ResolveEvaluable(Node operhand, IReturnableCreationStatement creation, Scope scope, TypeSearcherService searcherService)
        {
            switch(operhand.NodeType)
            {
                case NodeType.Default:
                    return creation.CreationType;
                default:
                    DescriberEnum describer = 0;

                    if (creation.Describer.CheckDescription(DescriberEnum.Static))
                        describer |= DescriberEnum.Static;
                    

                    return ResolveEvaluable(operhand, scope, searcherService);
            }
        }

        //implement each kind of evaluable you can encounter (this, default, input, etc)
        private DataType ResolveEvaluable(Node operhand, Scope scope, TypeSearcherService searcherService)
        {
            DataType result;
            switch (operhand.NodeType)
            {
                case NodeType.Dot:
                case NodeType.Variable:
                case NodeType.FunctionCall:
                case NodeType.ConstructorCall:
                    var entity = ReferenceEntity(operhand, scope, searcherService);
                    result = entity == null ? null : entity.CreationType;
                    break;
                case NodeType.Input:
                    return defaultNameSpace.GetInternalDataType(ConstantType.String);
                case NodeType.This:
                    var thisNode = (ThisNode)operhand;
                    switch (thisNode.Reference.NodeType)
                    {
                        case NodeType.Dot:
                            var dot = (DotExpression)thisNode.Reference;
                            var creation = ReferenceInClassVariable((IdentifierNode)dot.LHS, scope);

                            return ReferenceExternalVariable(creation, dot.RHS, scope, searcherService).CreationType;
                        default:
                            var identifer = (IdentifierNode)thisNode.Reference;
                            return ReferenceInClassVariable(identifer, scope).CreationType;
                    }
                case NodeType.Parent:
                    var parentNode = (ParentNode)operhand;
                    //I DONT KNWO WHAT IM DOINGJKHQKJFGHJAGS
                case NodeType.Constant:
                    var constant = (ConstantValueNode)operhand;
                    result = constant.Token.ConstantType == ConstantType.Null ? null : defaultNameSpace.GetInternalDataType(constant.Token.ConstantType);
                    break;
                case NodeType.Cast:
                    var asConversion = (CastExpression)operhand;

                    var value = (IExplicitContainer)ResolveEvaluable(asConversion.LHS, scope, searcherService);
                    var type = FindReferencedType(searcherService, (TypeNode)asConversion.RHS);

                    if (value == null || type == null)
                    {
                        result = null;
                        break;
                    }

                    var cast = value.TryFindExplicitCastDeclaration(type);
                    if(!cast.CheckDescription(DescriberEnum.Public))
                    {
                        //exception
                    }

                    result = cast == null ? null : cast.CreationType;
                    break;
                case NodeType.Unary:
                case NodeType.Binary:
                case NodeType.Ternary:
                    result = ResolveExpression(operhand, scope, searcherService);
                    break;
                default:
                    return null;
            }

            return result;
        }

        private DataType ResolveExpression(Node expression, Scope scope, TypeSearcherService searcherService)
        {
            switch (expression.NodeType)
            {
                case NodeType.Unary:
                    var unary = (UnaryExpression)expression;

                    var operhand = ResolveOperhand(unary.Operhand, scope, searcherService);
                    if (operhand == null)
                        return null;

                    var overload = operhand.TryFindOperatorOverloadDeclaration(unary.Operator, (DataType)operhand);

                    if(overload == null)
                    {
                        //exception
                    }
                    else if(!overload.CheckDescription(DescriberEnum.Public))
                    {
                        //exception
                    }
                    else if (overload.FunctionArguments[0] != operhand)
                    {
                        //exception
                    }
                    else
                        return overload.CreationType;

                    return null;
                case NodeType.Binary:
                    //after that for evlauting binary expressions, any of the 2 types in the expression can contain the required operator
                    //check in each and then return the suitable, fif the same exists in both, return an error
                    var binary = (BinaryExpression)expression;

                    var rhs = ResolveOperhand(binary.RHS, scope, searcherService);
                    var lhs = ResolveOperhand(binary.RHS, scope, searcherService);

                    if (rhs == null || lhs == null)
                        return null;

                    overload = lhs.TryFindOperatorOverloadDeclaration(binary.Operator, (DataType)lhs, (DataType)rhs);
                    if(overload != null)
                    {
                        var secondary = rhs.TryFindOperatorOverloadDeclaration(binary.Operator, (DataType)lhs, (DataType)rhs);
                        if(secondary != null)
                        {
                            //exception
                        }
                    }
                    else
                        overload = rhs.TryFindOperatorOverloadDeclaration(binary.Operator, (DataType)lhs, (DataType)rhs);

                    if (overload == null)
                    {
                        //exception
                    }
                    else if (!overload.CheckDescription(DescriberEnum.Public))
                    {
                        //exception
                    }

                    return overload.CreationType;
                default:
                    var ternary = (TernaryExpression)expression;

                    var condition = ResolveExpression(ternary.Condition, scope, searcherService);
                    if (condition == null)
                        return null;

                    if (condition != defaultNameSpace.GetInternalDataType(ConstantType.Boolean))
                    {
                        //exception
                    }

                    var trueValue = ResolveEvaluable(ternary.TrueExpression, scope, searcherService);
                    var falseValue = ResolveEvaluable(ternary.FalseExpression, scope, searcherService);

                    if (trueValue == null || falseValue == null)
                        return null;
                    if (trueValue != falseValue)
                    {
                        //exception
                    }
 

                    return trueValue;
            }
        }

        private IOperatorContainer ResolveOperhand(Node operhand, Scope scope, TypeSearcherService searcherService) => (IOperatorContainer)ResolveEvaluable(operhand, scope, searcherService);

        private IReturnableCreationStatement ReferenceEntity(Node name, Scope scope, TypeSearcherService searcherService)
        {
            switch (name.NodeType)
            {
                case NodeType.Variable:
                    return ReferenceInClassVariable((IdentifierNode)name, scope);
                case NodeType.FunctionCall:
                    return ResolveFunction((FunctionCallNode)name, scope, searcherService);
                case NodeType.ConstructorDeclaration:
                    return ResolveConstructorCall((ConstructorCallNode)name, scope, searcherService);
                case NodeType.Dot:
                    var dot = (DotExpression)name;
                    var lhs = dot.LHS;

                    IReturnableCreationStatement entity = null;
                    switch(lhs.NodeType)
                    {
                        case NodeType.This:
                            entity = ReferenceInClassVariable((IdentifierNode)lhs, scope);
                            break;
                        case NodeType.Variable:
                            entity = ReferenceInClassVariable((IdentifierNode)lhs, scope);
                            break;
                        case NodeType.FunctionCall:
                            entity = ResolveFunction((FunctionCallNode)lhs, scope, searcherService);
                            break;
                        case NodeType.ConstructorDeclaration:
                            entity = ResolveConstructorCall((ConstructorCallNode)lhs, scope, searcherService);
                            break;
                    }
                    if (entity == null)
                        return null;

                    return ReferenceExternalVariable(entity, dot.RHS, scope, searcherService);
            }

            return ReferenceExternalVariable(name, scope, searcherService);
        }

        private IMethod ResolveFunction(FunctionCallNode functionCall, Scope scope, TypeSearcherService searcherService)
        {
            var function = ReferenceInClassFunction(functionCall, scope, searcherService);
            if (function == null)
                return null;

            switch (function.ActionNodeType)
            {
                case ActionNodeEnum.LocalFunction:
                case ActionNodeEnum.GlobalFunction:
                case ActionNodeEnum.ExtensionFunction:
                    return function.CheckDescription(DescriberEnum.Public) ? (IMethod)function : null;
                default:
                    //exception
                    return null;
            }
        }

        private IMethod ResolveConstructorCall(ConstructorCallNode constructorCall, Scope scope, TypeSearcherService searcherService)
        {
            var type = FindReferencedType(searcherService, (TypeNode)constructorCall.Value);
            if (type == null)
                return null;

            var constructors = type.GetAllMembers(MemberEnum.Constructor);
            foreach (var constructor in constructors)
            {
                var converted = (IMethod)constructor;

                if (!converted.CheckDescription(DescriberEnum.Public))
                    continue;
                if (ValidateFunctionArguments(converted, constructorCall, scope, searcherService))
                    return converted;
            }

            return null;
        }

        private IReturnableCreationStatement ReferenceInClassVariable(IdentifierNode name, Scope scope)
        {
            IFunction current = (IFunction)scope.Parent;

            while (true)
            {
                var creationStatement = current.TryFindVariableCreation(name);

                if (creationStatement != null)
                    return creationStatement;
                else
                {
                    var parent = scope.Parent;
                    switch (parent.ActionNodeType)
                    {
                        case ActionNodeEnum.LocalFunction:
                            var localFunction = (ILocalFunction)parent;
                            var argument = localFunction.TryFindFunctionArgument(name);

                            if (argument != null)
                                return argument;
                            current = (IFunction)localFunction.Parent;
                            break;
                        default:
                            var globalFunction = (IGlobalFunction)parent;
                            argument = globalFunction.TryFindFunctionArgument(name);

                            if (globalFunction != null)
                                return argument;

                            var type = (IVariableContainer)globalFunction.Parent;
                            var global = type.TryFindVariableCreation(name);
                            return global;
                    }
                }
            }
        }

        private IFunction ReferenceInClassFunction(FunctionCallNode callNode, Scope scope, TypeSearcherService searcher)
        {
            var current = (IFunction)scope.Parent;
            var name = (IdentifierNode)callNode.Value;

            while (true)
            {
                var arguments = CreateFunctionArguments(searcher, scope, (FunctionCallArgumentsNode)callNode.Arguments);
                var creationStatement = current.TryFindMethodDeclaration(name, arguments);

                if (creationStatement != null)
                {
                    if (ValidateFunctionArguments(creationStatement, callNode, scope, searcher))
                        return creationStatement;
                }

                var parent = scope.Parent;
                switch (parent.ActionNodeType)
                {
                    case ActionNodeEnum.LocalFunction:
                        current = (IFunction)parent;
                        break;
                    default:
                        var type = ((IGlobalFunction)parent).Parent;
                        var global = type.TryFindMethodDeclaration(name, arguments);
                        if (global == null)
                            return type.TryFindMethodDeclaration(name, arguments);

                        return global;
                }
            }
        }

        private IReturnableCreationStatement ReferenceExternalVariable(Node name, Scope scope, TypeSearcherService searcherService)
        {
            var results = searcherService.ReferenceDataTypeCollection(name);
            var final = new List<IReturnableCreationStatement>();

            int count = results.Count;
            for (int i = 0; i < count; i++)
            {
                var current = results.Dequeue();

                var reference = ReferenceExternalVariable(current.Remaining, scope, searcherService);
                if (reference != null)
                    final.Add(reference);
            }

            switch (final.Count)
            {
                case 0:
                    result.Add(new NoReferenceToTypeException("", "", 0));
                    return null;
                case 1:
                    var entity = final[0];
                    if(!entity.CheckDescription(DescriberEnum.Public))
                    {
                        //exception
                        return null;
                    }
                    else if(!entity.CheckDescription(DescriberEnum.Static))
                    {
                        //exception
                        return null;
                    }
                    else
                        return final[0];
                default:
                    result.Add(new AmbigiousReferenceException("", "", 0));
                    return null;
            }
        }

        private IReturnableCreationStatement ReferenceExternalVariable(IReturnableCreationStatement start, Node name, Scope scope, TypeSearcherService searcherService)
        {
            var current = name;
            var type = start.CreationType;
            IReturnableCreationStatement entity = null; 

            bool breakOut = false;
            while (true)
            {
                switch(current.NodeType)
                {
                    case NodeType.Variable:
                        entity = ResolveEntity((IdentifierNode)current);
                        breakOut = true;
                        break;
                    case NodeType.FunctionCall:
                        var functionCall = (FunctionCallNode)current;
                        entity = TryFindMethod((IFunctionContainer<MethodDeclarationStmt, VoidDeclarationStmt>)type, (IdentifierNode)functionCall.Value, functionCall);
                        breakOut = true;
                        break;
                    case NodeType.Dot:
                        var dot = (DotExpression)current;

                        switch (dot.LHS.NodeType)
                        {
                            case NodeType.Variable:
                                entity = ResolveEntity((IdentifierNode)dot.LHS);
                                if (entity == null)
                                    return null;

                                type = entity.CreationType;
                                break;
                            case NodeType.FunctionCall:
                                functionCall = (FunctionCallNode)dot.LHS;
                                var function = TryFindMethod((IFunctionContainer<MethodDeclarationStmt, VoidDeclarationStmt>)type, (IdentifierNode)functionCall.Value, functionCall);
                                if (function == null)
                                    return null;

                                type = function.CreationType;
                                break;
                        }

                        current = dot.RHS;
                        break;
                }

                if(breakOut)
                {
                    if(!entity.CheckDescription(DescriberEnum.Public))
                    {
                        //exception
                        return null;
                    }

                    return entity;
                }

                IReturnableCreationStatement ResolveEntity(IdentifierNode identifier)
                {
                    switch (type.ActionNodeType)
                    {
                        case ActionNodeEnum.Enum:
                            return TryFindVariable((IVariableContainer)type, identifier);
                        case ActionNodeEnum.Class:
                        case ActionNodeEnum.Struct:
                            entity = TryFindVariable((IGeneralContainer)type, identifier);

                            if (entity == null)
                                return TryFindProperty((IGeneralContainer)type, identifier);

                            return entity;
                        default:
                            return TryFindProperty((IGeneralContainer)type, identifier);
                    }
                }

                IReturnableCreationStatement TryFindVariable(IVariableContainer container, IdentifierNode identifier) => container.TryFindVariableCreation(identifier);

                IReturnableCreationStatement TryFindProperty(IPropertyContainer container, IdentifierNode identifier) => container.TryFindPropertyCreation(identifier);

                MethodDeclarationStmt TryFindMethod(IFunctionContainer<MethodDeclarationStmt, VoidDeclarationStmt> container, IdentifierNode identifier, FunctionCallNode callNode)
                {
                    switch (type.ActionNodeType)
                    {
                        case ActionNodeEnum.Class:
                        case ActionNodeEnum.Struct:
                        case ActionNodeEnum.Interface:
                            var arguments = CreateFunctionArguments(searcherService, scope, (FunctionCallArgumentsNode)callNode.Arguments);
                            var function = container.TryFindMethodDeclaration(identifier, arguments);

                            if (function == null)
                                return null;
                            if (!ValidateFunctionArguments(function, callNode, scope, searcherService))
                                return null;

                            return function;
                    }

                    return null;
                }
            }
        }

        private FunctionDeclArgs CreateFunctionArguments(TypeSearcherService searcherService, Scope scope, FunctionDeclarationArgumentsNode argumentsNode)
        {
            var arguments = new FunctionDeclArgs();

            for (int i = 0; i < argumentsNode.ChildCount; i++)
            {
                var child = (FunctionDeclarationArgumentNode)argumentsNode[i];

                FunctionArgumentDeclarationStmt argument;
                var name = (IdentifierNode)child.Name;
                var describer = describerService.AnalyseDescriber((DescriberNode)child.Describer);

                if (!describer.ValidateDescriber(DescriberEnum.ReferenceModifiers))
                {
                    result.Add(new InvalidDescriberException(name.Value, DescriberEnum.ReferenceModifiers, describer));
                    continue;
                }

                var type = FindReferencedType(searcherService, (TypeNode)child.Type);
                if (type == null)
                    continue;

                if (child.DefaultValue == null)
                    argument = new FunctionArgumentDeclarationStmt(type, name, describer);
                else
                {
                    var evaluated = ResolveEvaluable(child.DefaultValue, scope, searcherService);

                    if (evaluated != type)
                    {
                        //exception
                        continue;
                    }

                    argument = new FunctionArgumentInitialisationStmt(type, name, describer, (ExpressionNode)child.DefaultValue);
                }

                arguments.Add(name.Value, argument);
            }

            return arguments;
        }

        private FunctionCallArgs CreateFunctionArguments(TypeSearcherService subTypeSearcher, Scope scope, FunctionCallArgumentsNode argumentsNode)
        {
            var arguments = new FunctionCallArgs();

            for (int i = 0; i < argumentsNode.ChildCount; i++)
            {
                var child = (FunctionCallArgumentNode)argumentsNode[i];

                
                var describer = describerService.AnalyseDescriber((DescriberNode)child.Describer);

                if (!describer.ValidateDescriber(DescriberEnum.ReferenceModifiers))
                {
                    result.Add(new InvalidDescriberException($"Function Argument [{i + 1}]", DescriberEnum.ReferenceModifiers, describer));
                    continue;
                }

                var evaluated = ResolveEvaluable(child.Value, scope, subTypeSearcher);
                if (evaluated == null)
                    continue;

                var argument = new FunctionCallArgument(evaluated, describer);
                
                arguments.Add(i, argument);
            }

            return arguments;
        }

        private bool ValidateFunctionArguments(IFunction functionCall, BaseFunctionCallNode callNode, Scope scope, TypeSearcherService searcherService)
        {
            int i;
            for(i = 0; i < functionCall.FunctionArguments.Count; i++)
            {
                if(i >= callNode.Arguments.ChildCount)
                {
                    result.Add(new FunctionArgumentsNotFoundException(functionCall.Name));
                    return false;
                }

                var arg = callNode.Arguments[i];

                var reference = ResolveEvaluable(arg, scope, searcherService);
                if(reference == null)
                    continue;

                var argument = functionCall.FunctionArguments[i];
                if(argument != reference)//check for possible comparisons as well
                {
                    result.Add(new FunctionArgumentTypeMismatchException(functionCall.Name, argument.Name));
                    continue;
                }
            }

            if(i < callNode.ChildCount)
            {
                result.Add(new FunctionArgumentsNotFoundException(functionCall.Name));
                return false;
            }

            return i == callNode.ChildCount;
        }
    }
}
