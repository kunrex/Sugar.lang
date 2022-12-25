using System;
using System.Collections.Generic;

using Sugar.Language.Semantics.Analysis;
using Sugar.Language.Semantics.Services.Interfaces;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Namespaces;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global;
using Sugar.Language.Parsing.Nodes.Statements;
using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;
using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation;
using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions;
using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Functions.Declarations;
using Sugar.Language.Parsing.Nodes.Types;
using Sugar.Language.Parsing.Nodes.Types.Subtypes;
using Sugar.Language.Parsing.Nodes.Types.Enums;
using Sugar.Language.Semantics.Services.Implementations.Structures;
using Sugar.Language.Parsing.Nodes.Describers;
using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation.Local.FunctionArguments;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Local;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.PropertyCreation;
using Sugar.Language.Parsing.Nodes.Expressions;
using Sugar.Language.Semantics.ActionTrees;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.PropertyCreation.PropertyIdentifiers;
using Sugar.Language.Parsing.Nodes.Statements.VariableCreation;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation.Local;
using Sugar.Language.Parsing.Nodes.Expressions.Associative;
using Sugar.Language.Semantics.ActionTrees.CreationStatements;
using Sugar.Language.Semantics.ActionTrees.Interfaces;
using Sugar.Language.Exceptions.Analytics.ClassMemberCreation.SubTypeSearching;
using Sugar.Language.Parsing.Nodes.Functions.Calling;
using Sugar.Language.Exceptions.Analytics.Referencing;

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

        private void Validate(StructType dataType)
        {
            var subTypeSearcher = new TypeSearcherService(dataType, defaultNameSpace);
            var functions = dataType.GetAllMembers(MemberEnum.Void);

            foreach (var function in functions)
            {
                var converted = (IFunction)function;

                var body = converted.Scope.Body;
                if (body.NodeType == NodeType.Scope)
                    CreateLocalMembers(converted.Scope, subTypeSearcher);
                else if(body.NodeType == NodeType.FunctionDeclaration)
                {
                    CreateLocalFunction((FunctionDeclarationNode)body, converted, subTypeSearcher);
                    //probably throw a warning
                }
            }

            var properties = dataType.GetAllMembers(MemberEnum.Properties);

            foreach(var property in properties)
            {
                var declaration = (IProperty)property;

                switch (declaration.PropertyType)
                {
                    case PropertyTypeEnum.Get:
                        CreateLocalMembers(declaration.GetExpression.Scope, subTypeSearcher);
                        break;
                    case PropertyTypeEnum.Set:
                        CreateLocalMembers(declaration.SetExpression.Scope, subTypeSearcher);
                        break;
                    case PropertyTypeEnum.GetSet:
                        CreateLocalMembers(declaration.GetExpression.Scope, subTypeSearcher);
                        CreateLocalMembers(declaration.SetExpression.Scope, subTypeSearcher);
                        break;
                }
            }

            foreach (var function in functions)
            {
                var converted = (IFunction)function;

                var scope = converted.Scope;
                var body = scope.Body;
    
                if (body.NodeType == NodeType.Scope)
                    foreach(var child in scope.Body.GetChildren())
                    {
                        switch(child.NodeType)
                        {
                            case NodeType.Scope:
                            case NodeType.FunctionDeclaration:
                                break;
                            case NodeType.Declaration:
                            case NodeType.Initialise:
                                CreatLocalVariable((DeclarationNode)child, scope, subTypeSearcher);
                                break;
                            case NodeType.Assignment:
                                var assignment = (AssignmentNode)child;

                                var value = assignment.Value;
                                var variable = ReferenceEntity(value, scope, subTypeSearcher);
                                Console.WriteLine(variable == null);
                                break;
                        }
                    }
            }
        }

        private void CreateLocalMembers(Scope parent, TypeSearcherService subTypeSearcher)
        {
            var body = parent.Body;

            if (body.NodeType == NodeType.Scope)
                foreach (var child in body.GetChildren())
                    switch (child.NodeType)
                    {
                        case NodeType.Scope:
                            var scope = new Scope(child);
                            CreateLocalMembers(scope, subTypeSearcher);

                            parent.AddScope(scope);
                            scope.SetParent(parent);
                            break;
                        case NodeType.FunctionDeclaration:
                            CreateLocalFunction((FunctionDeclarationNode)child, parent, subTypeSearcher);
                            break;
                    }
        }

        private void CreatLocalVariable(DeclarationNode declaration, Scope scope, TypeSearcherService subTypeSearcher)
        {
            var name = (IdentifierNode)declaration.Name;
            var type = FindReferencedType(subTypeSearcher, (TypeNode)declaration.Type);
            if (type == null)
                return;

            var describer = describerService.AnalyseDescriber((DescriberNode)declaration.Describer);

            switch(declaration.NodeType)
            {
                case NodeType.Declaration:
                    scope.AddDeclaration(new LocalVariableDeclarationStmt(type, name, describer));
                    break;
                case NodeType.Initialise:
                    scope.AddDeclaration(new LocalVariableInitialisationStmt(type, name, describer, (ExpressionNode)((InitializeNode)declaration).Value));
                    break;
            }
        }

        private void CreateLocalFunction(FunctionDeclarationNode function, IFunctionContainer<LocalMethodCreationStmt, LocalVoidDeclarationStmt> parent, TypeSearcherService subTypeSearcher)
        {
            var name = (IdentifierNode)function.Name;
            var functionInfo = GatherArguments(function, subTypeSearcher, (TypeNode)function.Type);

            IFunction localFunction;
            if (functionInfo.ReturnType == null)
            {
                var localVoid = new LocalVoidDeclarationStmt(name, functionInfo.Describer, functionInfo.Arguments, functionInfo.Body);
                parent.AddDeclaration(localVoid);

                localFunction = localVoid;
            }
            else
            {
                var localMethod = new LocalMethodCreationStmt(functionInfo.ReturnType, name, functionInfo.Describer, functionInfo.Arguments, functionInfo.Body);
                parent.AddDeclaration(localMethod);

                localFunction = localMethod;
            }

            CreateLocalMembers(localFunction.Scope, subTypeSearcher);
        }

        private FunctionInfo GatherArguments(BaseFunctionDeclarationNode baseFunction, TypeSearcherService subTypeSearcher, TypeNode type) => new FunctionInfo(baseFunction.Body,
                                    FindReferencedType(subTypeSearcher, type),
                                    new Describer(0),
                                    CreateArguments(subTypeSearcher, (FunctionDeclarationArgumentsNode)baseFunction.Arguments));

        private DataType FindReferencedType(TypeSearcherService subTypeSearcher, TypeNode type)
        {
            switch (type.Type)
            {
                case TypeNodeEnum.BuiltIn:
                    return defaultNameSpace.GetInternalDataType(type.ReturnType());
                case TypeNodeEnum.Constructor:
                    return FindReference(((ConstructorTypeNode)type).ConstructorReturnType);
                case TypeNodeEnum.Void:
                    return null;
                default:
                    return FindReference(((CustomTypeNode)type).CustomType);
            }

            DataType FindReference(Node type)
            {
                var results = subTypeSearcher.ReferenceDataTypeCollection(type);

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
        }

        private FunctionArguments CreateArguments(TypeSearcherService subTypeSearcher, FunctionDeclarationArgumentsNode argumentsNode)
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
                    Console.WriteLine("Invalid Describer for argument");
                    continue;
                }

                var type = FindReferencedType(subTypeSearcher, (TypeNode)child.Type);
                if (type == null)
                    continue;

                if (child.DefaultValue == null)
                    argument = new FunctionArgumentDeclarationStmt(type, name, describer);
                else
                    argument = new FunctionArgumentInitialisationStmt(type, name, describer, (ExpressionNode)child.DefaultValue);

                arguments.Add(name.Value, argument);
            }

            return arguments;
        }

        private IReturnableCreationStatement ReferenceEntity(Node name, Scope scope, TypeSearcherService searcherService)
        {
            switch (name.NodeType)
            {
                case NodeType.Variable:
                    return ReferenceInClassVariable((IdentifierNode)name, scope);
                case NodeType.FunctionCall:
                    var function = ReferenceInClassFunction((FunctionCallNode)name, scope, searcherService);
                    switch(function.ActionNodeType)
                    {
                        case ActionNodeEnum.LocalFunction:
                        case ActionNodeEnum.GlobalFunction:
                        case ActionNodeEnum.ExtensionFunction:
                            return (IReturnableCreationStatement)function;
                    }
                    break;
                case NodeType.Dot:
                    var dot = (DotExpression)name;
                    var rhs = dot.RHS;

                    switch(dot.RHS.NodeType)
                    {
                        case NodeType.Variable:
                            var entity = ReferenceInClassVariable((IdentifierNode)rhs, scope);

                            if (entity == null)
                                return null;
                            return ReferenceExternalVariable(entity, dot.LHS, scope, searcherService);
                        case NodeType.FunctionCall:
                            function = ReferenceInClassFunction((FunctionCallNode)name, scope, searcherService);
                            if (function == null)
                                return null;

                            switch (function.ActionNodeType)
                            {
                                case ActionNodeEnum.LocalFunction:
                                case ActionNodeEnum.GlobalFunction:
                                case ActionNodeEnum.ExtensionFunction:
                                    return ReferenceExternalVariable((IReturnableCreationStatement)function, dot.LHS, scope, searcherService);
                            }
                            break;
                    }
                    break;
            }

            return ReferenceExternalVariable(name, scope, searcherService);
        }

        //this should work but its ugly, refractor it a bit
        private IReturnableCreationStatement ReferenceExternalVariable(IReturnableCreationStatement start, Node name, Scope scope, TypeSearcherService searcherService)
        {
            var current = name;
            var type = start.CreationType;
            IReferencableEntity entity = null;

            while (true)
            {
                switch(current.NodeType)
                {
                    case NodeType.Variable:
                        var identifier = (IdentifierNode)current;

                        switch (type.ActionNodeType)
                        {
                            case ActionNodeEnum.Enum:
                                return TryFindVariable((IVariableContainer)type, identifier);
                            case ActionNodeEnum.Interface:
                                return TryFindProperty((IGeneralContainer)type, identifier);
                            case ActionNodeEnum.Class:
                            case ActionNodeEnum.Struct:
                                entity = TryFindVariable((IGeneralContainer)type, identifier);

                                if (entity == null)
                                    return TryFindProperty((IGeneralContainer)type, identifier);

                                return entity;
                        }
                        break;
                    case NodeType.FunctionCall:
                        var functionCall = (FunctionCallNode)current;
                        return TryFindFunction((IFunctionContainer<MethodDeclarationStmt, VoidDeclarationStmt>)type, (IdentifierNode)functionCall.Value, functionCall);
                    case NodeType.Dot:
                        var dot = (DotExpression)current;

                        switch (dot.RHS.NodeType)
                        {
                            case NodeType.Variable:
                                var rhs = (IdentifierNode)dot.RHS;
                                switch (type.ActionNodeType)
                                {
                                    case ActionNodeEnum.Enum:
                                        entity = TryFindVariable((IVariableContainer)type, rhs);
                                        break;
                                    case ActionNodeEnum.Interface:
                                        entity = TryFindProperty((IGeneralContainer)type, rhs);
                                        break;
                                    case ActionNodeEnum.Class:
                                    case ActionNodeEnum.Struct:
                                        entity = TryFindVariable((IGeneralContainer)type, rhs);

                                        if (entity == null)
                                            entity = TryFindProperty((IGeneralContainer)type, rhs);
                                        break;
                                }

                                if (entity == null)
                                    return null;

                                current = dot.LHS;
                                type = entity.CreationType;
                                break;
                            case NodeType.FunctionCall:
                                functionCall = (FunctionCallNode)dot.RHS;
                                var function = TryFindFunction((IFunctionContainer<MethodDeclarationStmt, VoidDeclarationStmt>)type, (IdentifierNode)functionCall.Value, functionCall);

                                if (function == null)
                                    return null;

                                current = dot.LHS;
                                type = function.CreationType;
                                break;
                        }
                        break;
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

        private IReturnableCreationStatement ReferenceExternalVariable(Node name, Scope scope, TypeSearcherService searcherService)
        {
            var results = searcherService.ReferenceDataTypeCollection(name);
            var final = new List<IReturnableCreationStatement>();

            int count = results.Count;
            for(int i = 0; i < count; i++)
            {
                var current = results.Dequeue();

                var reference = ReferenceExternalVariable(current.Remaining, scope, searcherService);
                if (reference != null)
                    final.Add(reference);
            }

            switch(final.Count)
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

        private IReferencableEntity ReferenceInClassVariable(IdentifierNode name, Scope scope)
        {
            IFunction current = (IFunction)scope.Parent;

            while(true)
            {
                var creationStatement = current.TryFindVariableCreation(name);

                if (creationStatement != null)
                    return creationStatement;
                else
                {
                    var parent = scope.Parent;
                    switch(parent.ActionNodeType)
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
            IFunction current = (IFunction)scope.Parent;

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
                        {
                            var globalVoid = type.TryFindMethodDeclaration(name);

                            return globalVoid;
                        }

                        return global;
                }
            }
        }

        private bool ValidateFunctionArguments(IFunction functionCall, FunctionCallNode callNode, Scope scope, TypeSearcherService searcherService)
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

                var reference = ReferenceEntity(arg, scope, searcherService);
                if(reference == null)
                    continue;

                var argument = functionCall.FunctionArguments[i];
                if(argument.CreationType != reference.CreationType)//check for possible comparisons as well
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
