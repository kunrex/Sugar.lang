﻿using System;
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
using Sugar.Language.Parsing.Nodes.Types.Subtypes;
using Sugar.Language.Parsing.Nodes.Functions.Calling;
using Sugar.Language.Parsing.Nodes.Expressions.Operative;
using Sugar.Language.Parsing.Nodes.Functions.Declarations;
using Sugar.Language.Parsing.Nodes.Expressions.Associative;
using Sugar.Language.Parsing.Nodes.Statements.VariableCreation;
using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;

using Sugar.Language.Semantics.Analysis;
using Sugar.Language.Semantics.ActionTrees;
using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.Services.Interfaces;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Namespaces;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.CreationStatements;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Local;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.PropertyCreation;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation.Local;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation.Local.FunctionArguments;

using Sugar.Language.Exceptions.Analytics.Referencing;
using Sugar.Language.Exceptions.Analytics.ClassMemberCreation;
using Sugar.Language.Exceptions.Analytics.ClassMemberCreation.Statements;
using Sugar.Language.Exceptions.Analytics.ClassMemberCreation.SubTypeSearching;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global.Conversion;

namespace Sugar.Language.Semantics.Services.Implementations
{
    internal sealed class StatementService : IStatementService
    {
        private readonly DefaultNameSpaceNode defaultNameSpace;
        private readonly CreatedNameSpaceCollectionNode createdNameSpaces;

        private readonly SemanticsResult result;

        private readonly DescriberService describerService;

        public StatementService(DefaultNameSpaceNode _default, CreatedNameSpaceCollectionNode _collection)
        {
            defaultNameSpace = _default;
            createdNameSpaces = _collection;

            describerService = new DescriberService();

            result = new SemanticsResult("Statement Validation");
        }

        public SemanticsResult Validate()
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

                            var variable = ReferenceEntity(assignment.Value, scope, searcherService);
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
                        case NodeType.FunctionDeclaration:
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
            if (parent.TryFindFunctionDeclaration(name) == null)
            {
                if(parent.TryFindMethodDeclaration(name) != null)
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
            var arguments = CreateArguments(subTypeSearcher, parent, (FunctionDeclarationArgumentsNode)function.Arguments);

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

        private FunctionArguments CreateArguments(TypeSearcherService searcherService, Scope scope, FunctionDeclarationArgumentsNode argumentsNode)
        {
            var arguments = new FunctionArguments();

            for (int i = 0; i < argumentsNode.ChildCount; i++)
            {
                var child = (FunctionDeclarationArgumentNode)argumentsNode[i];

                FunctionArgumentDeclarationStmt argument;
                var name = (IdentifierNode)child.Name;
                var describer = describerService.AnalyseDescriber((DescriberNode)child.Describer);

                if (!describer.ValidateAccessor(DescriberEnum.ReferenceModifiers))
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

        private DataType FindReferencedType(TypeSearcherService searcherService, TypeNode type)
        {
            switch (type.Type)
            {
                case TypeNodeEnum.BuiltIn:
                    return defaultNameSpace.GetInternalDataType(type.ReturnType());
                case TypeNodeEnum.Constructor:
                    return FindReferencedType(searcherService, ((ConstructorTypeNode)type).ConstructorReturnType);
                default:
                    return FindReferencedType(searcherService, ((CustomTypeNode)type).CustomType);
            }
        }

        private DataType FindReferencedType(TypeSearcherService searcherService, Node type)
        {
            var results = searcherService.ReferenceDataTypeCollection(type);

            int count = results.Count;
            for (int i = 0; i < count; i++)
            {
                var result = results.Dequeue();

                if (result.Remaining != null && result.ResultEnum != ActionNodeEnum.Namespace)
                    results.Enqueue(result);
            }

            switch (results.Count)
            {
                case 0:
                    result.Add(new NoReferenceToTypeException(type.ToString(), "", 0));
                    return null;
                case 1:
                    return (DataType)results.Dequeue().Result;
                default:
                    result.Add(new AmbigiousReferenceException(type.ToString(), "", 0));
                    return null;
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
                case NodeType.Constant:
                    var constant = (ConstantValueNode)operhand;
                    result = constant.Token.ConstantType == ConstantType.Null ? null : defaultNameSpace.GetInternalDataType(constant.Token.ConstantType);
                    break;
                case NodeType.Cast:
                    var asConversion = (CastExpression)operhand;

                    var value = ResolveEvaluable(asConversion.LHS, scope, searcherService);
                    var type = FindReferencedType(searcherService, (TypeNode)asConversion.RHS);

                    if (value == null || type == null)
                        return null;

                    var conversions = value.GetAllMembers(MemberEnum.ExplicitCast);
                    foreach(var conversion in conversions)
                    {
                        var converted = (ExplicitCastDeclarationStmt)conversion;

                        if (converted.CreationType == type)
                            return type;
                    }
                    return null;
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

        private IOperatorContainer ResolveOperhand(Node operhand, Scope scope, TypeSearcherService searcherService) => (IOperatorContainer)ResolveEvaluable(operhand, scope, searcherService);

        private DataType ResolveExpression(Node expression, Scope scope, TypeSearcherService searcherService)
        {
            switch (expression.NodeType)
            {
                case NodeType.Unary:
                    var unary = (UnaryExpression)expression;

                    var operhand = ResolveOperhand(unary.Operhand, scope, searcherService);
                    var overload = operhand.TryFindOperatorOverloadDeclaration(unary.Operator);

                    if(overload == null)
                    {
                        //exception
                    }
                    else if (overload.FunctionArguments[0].CreationType != operhand)
                    {
                        //exception
                    }
                    else
                        return overload.CreationType;

                    return null;
                case NodeType.Binary:
                    //alright basically, every where we create functions we dont check for duplicates based in argument types
                    //you need to fix that, in class member service and here for local functions also
                    //you need to do the same for operator overloading and cast decalartions with their own special checks
                    //in unary operator, the parameter must be the type containing the overload
                    //in binary, one of the parameters must be the type containing the overload
                    //in cast, either the return type or the parameter must be the type containing the declartion
                    //after that for evlauting binary expressions, any of the 2 types in the expression can contain the required operator
                    //check in each and then return the suitable, fif the same exists in both, return an error
                    var binary = (BinaryExpression)expression;

                    var rhs = ResolveOperhand(binary.RHS, scope, searcherService);
                    var lhs = ResolveOperhand(binary.RHS, scope, searcherService);

                    IOperatorContainer other, basic;
                    if (binary.Operator.LeftAssociative)
                    {
                        basic = lhs;
                        other = rhs;
                    }
                    else
                    {
                        basic = rhs;
                        other = lhs;
                    }

                    overload = basic.TryFindOperatorOverloadDeclaration(binary.Operator);

                    if (overload == null)
                    {
                        //exception
                    }
                    else 
                    {
                        if (overload.FunctionArguments[0].CreationType != basic)
                        {
                            if(overload.FunctionArguments[1].CreationType != )
                        }
                    }
                    else
                        return overload.CreationType;

                    return null;
                default:
                    var ternary = (TernaryExpression)expression;

                    var condition = ResolveExpression(ternary.Condition, scope, searcherService);
                    if (condition != defaultNameSpace.GetInternalDataType(ConstantType.Boolean))
                    {
                        //exception
                    }

                    var trueValue = ResolveEvaluable(ternary.TrueExpression, scope, searcherService);
                    var falseValue = ResolveEvaluable(ternary.FalseExpression, scope, searcherService);

                    if (trueValue != falseValue)
                    {
                        //exception
                    }

                    return trueValue;
            }
        }

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

        private IReturnableCreationStatement ResolveFunction(FunctionCallNode functionCall, Scope scope, TypeSearcherService searcherService)
        {
            var function = ReferenceInClassFunction(functionCall, scope, searcherService);
            if (function == null)
                return null;

            switch (function.ActionNodeType)
            {
                case ActionNodeEnum.LocalFunction:
                case ActionNodeEnum.GlobalFunction:
                case ActionNodeEnum.ExtensionFunction:
                    return (IReturnableCreationStatement)function;
                default:
                    //exception
                    return null;
            }
        }

        private IReturnableCreationStatement ResolveConstructorCall(ConstructorCallNode constructorCall, Scope scope, TypeSearcherService searcherService)
        {
            var type = FindReferencedType(searcherService, constructorCall.Value);
            if (type == null)
                return null;

            var constructors = type.GetAllMembers(MemberEnum.Constructor);
            foreach (var constructor in constructors)
            {
                var converted = (IFunction)constructor;
                if (ValidateFunctionArguments(converted, constructorCall, scope, searcherService))
                    return (IReturnableCreationStatement)converted;
            }

            return null;
        }

        private void ResolveDataTypeConversion(DataType type1, DataType type2)
        {
            
        }

        private bool TryImplicityConvert(DataType type, DataType to)
        {
            var conversions = type.GetAllMembers(MemberEnum.ImplicitCast);
            foreach (var conversion in conversions)
            {
                var converted = (ExplicitCastDeclarationStmt)conversion;

                if (converted.CreationType == to)
                    return true;
            }

            return false;
        }

        private IReferencableEntity ReferenceInClassVariable(IdentifierNode name, Scope scope)
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
                var creationStatement = current.TryFindFunctionDeclaration(name);

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
                        var global = type.TryFindFunctionDeclaration(name);
                        if (global == null)
                            return type.TryFindMethodDeclaration(name);

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
                    return final[0];
                default:
                    result.Add(new AmbigiousReferenceException("", "", 0));
                    return null;
            }
        }

        private IReturnableCreationStatement ReferenceExternalVariable(IReturnableCreationStatement start, Node name, Scope scope, TypeSearcherService searcherService)
        {
            var current = name;
            IReferencableEntity entity;
            var type = start.CreationType;

            while (true)
            {
                switch(current.NodeType)
                {
                    case NodeType.Variable:
                        return ResolveEntity((IdentifierNode)current);
                    case NodeType.FunctionCall:
                        var functionCall = (FunctionCallNode)current;
                        return TryFindFunction((IFunctionContainer<MethodDeclarationStmt, VoidDeclarationStmt>)type, (IdentifierNode)functionCall.Value, functionCall);
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
                                var function = TryFindFunction((IFunctionContainer<MethodDeclarationStmt, VoidDeclarationStmt>)type, (IdentifierNode)functionCall.Value, functionCall);
                                if (function == null)
                                    return null;

                                type = function.CreationType;
                                break;
                        }

                        current = dot.RHS;
                        break;
                }

                IReferencableEntity ResolveEntity(IdentifierNode identifier)
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

                IReferencableEntity TryFindVariable(IVariableContainer container, IdentifierNode identifier) => container.TryFindVariableCreation(identifier);

                IReferencableEntity TryFindProperty(IPropertyContainer container, IdentifierNode identifier) => container.TryFindPropertyCreation(identifier);

                MethodDeclarationStmt TryFindFunction(IFunctionContainer<MethodDeclarationStmt, VoidDeclarationStmt> container, IdentifierNode identifier, FunctionCallNode callNode)
                {
                    switch (type.ActionNodeType)
                    {
                        case ActionNodeEnum.Class:
                        case ActionNodeEnum.Struct:
                        case ActionNodeEnum.Interface:
                            var function = container.TryFindFunctionDeclaration(identifier);

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
                if(argument.CreationType != reference)//check for possible comparisons as well
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
