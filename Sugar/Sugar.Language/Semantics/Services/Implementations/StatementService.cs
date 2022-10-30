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
            var subTypeSearcher = new SubTypeSearcherService(dataType, defaultNameSpace, createdNameSpaces);
            var functions = dataType.GetAllMembers(MemberEnum.Functions);

            foreach (var function in functions)
            {
                var converted = (IFunction)function;

                var body = converted.Scope.Body;
                if (body.NodeType == NodeType.Scope)
                {
                    foreach (var child in body.GetChildren())
                        if (child.NodeType == NodeType.FunctionDeclaration)
                            CreateLocalFunction((FunctionDeclarationNode)child, converted, subTypeSearcher);
                }
                else if(body.NodeType == NodeType.FunctionDeclaration)
                {
                    CreateLocalFunction((FunctionDeclarationNode)body, converted, subTypeSearcher);
                }
            }

            var properties = dataType.GetAllMembers(MemberEnum.Property);

            foreach(var property in properties)
            {
                var declaration = (PropertyDeclarationStmt)property;

                switch(declaration.PropertyType)
                {
                    case PropertyTypeEnum.Get:
                        CreateLocalMembers(declaration.GetExpression.Scope);
                        break;
                    case PropertyTypeEnum.Set:
                        CreateLocalMembers(declaration.SetExpression.Scope);
                        break;
                    case PropertyTypeEnum.GetSet:
                        CreateLocalMembers(declaration.GetExpression.Scope);
                        CreateLocalMembers(declaration.SetExpression.Scope);
                        break;
                }

                void CreateLocalMembers(Scope parent)
                {
                    var body = parent.Body;

                    if (body.NodeType == NodeType.Scope)
                        foreach (var child in body.GetChildren())
                            if (child.NodeType == NodeType.FunctionDeclaration)
                                CreateLocalFunction((FunctionDeclarationNode)child, parent, subTypeSearcher);
                }
            }    
        }

        //ScopeNode been created, implement it in all functions and prpperties 
        //all entities that require scopes can use this class internally, so functions included as well
        //in properties, each Propertyidentifier (get, set) has its own ScopeNode child,
        //the Set node has its own internal assignment of value to the specified data type of the property :)

        //creating local functions should now work recursiveley
        //implement this for properties and indexers as well
        //then move onto variables
        private void CreateLocalFunction(FunctionDeclarationNode function, IFunctionContainer<LocalMethodCreationStmt, LocalVoidDeclarationStmt> parent, SubTypeSearcherService subTypeSearcher)
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

            var body = localFunction.Scope.Body;
            if (body.NodeType == NodeType.Scope)
                foreach (var child in body.GetChildren())
                    if (child.NodeType == NodeType.FunctionDeclaration)
                    {
                        CreateLocalFunction((FunctionDeclarationNode)child, localFunction, subTypeSearcher);
                    }
        }

        private FunctionInfo GatherArguments(BaseFunctionDeclarationNode baseFunction, SubTypeSearcherService subTypeSearcher, TypeNode type) => new FunctionInfo(baseFunction.Body,
                                    FindReferencedType(subTypeSearcher, type),
                                    new Describer(0),
                                    CreateArguments(subTypeSearcher, (FunctionDeclarationArgumentsNode)baseFunction.Arguments));

        private FunctionInfo GatherArguments(BaseFunctionDeclarationNode baseFunction, SubTypeSearcherService subTypeSearcher) => new FunctionInfo(baseFunction.Body,
                                    null,
                                    new Describer(0),//actually check for an error here thanks
                                    CreateArguments(subTypeSearcher, (FunctionDeclarationArgumentsNode)baseFunction.Arguments));

        private DataType FindReferencedType(SubTypeSearcherService subTypeSearcher, TypeNode type)
        {
            switch (type.Type)
            {
                case TypeNodeEnum.BuiltIn:
                    return defaultNameSpace.GetInternalDataType(type.ReturnType());
                case TypeNodeEnum.Constructor:
                    return subTypeSearcher.TryFindReferencedType((CustomTypeNode)((ConstructorTypeNode)type).ConstructorReturnType);
                case TypeNodeEnum.Void:
                    return null;
                default:
                    return subTypeSearcher.TryFindReferencedType(((CustomTypeNode)type).CustomType);
            }
        }

        private FunctionArguments CreateArguments(SubTypeSearcherService subTypeSearcher, FunctionDeclarationArgumentsNode argumentsNode)
        {
            var arguments = new FunctionArguments();

            for (int i = 0; i < argumentsNode.ChildCount; i++)
            {
                var child = (FunctionDeclarationArgumentNode)argumentsNode[i];

                IFunctionArgument argument;
                var name = (IdentifierNode)child.Name;
                var describer = describerService.AnalyseDescriber((DescriberNode)child.Describer);

                if (!describer.ValidateAccessor(DescriberEnum.ReferenceModifiers))
                {
                    Console.WriteLine("Invalid Describer for argument");
                    continue;
                }

                if (child.DefaultValue == null)
                    argument = new FunctionArgumentDeclarationStmt(FindReferencedType(subTypeSearcher, (TypeNode)child.Type), name, describer);
                else
                    argument = new FunctionArgumentInitialisationStmt(FindReferencedType(subTypeSearcher, (TypeNode)child.Type), name, describer, (ExpressionNode)child.DefaultValue);

                arguments.Add(name.Value, argument);
            }

            return arguments;
        }

        /*private VariableCreationStmt ReferenceFinalVariable(Node node, IVariableContainer start)
        {
            IVariableContainer current = start;

            while(true)
            {
                switch(node.NodeType)
                {
                    case Parsing.Nodes.Enums.NodeType.Dot:
                        break;
                    case Parsing.Nodes.Enums.NodeType.Variable:
                        var identifer = (IdentifierNode)node;
                        Console.WriteLine(identifer.Value);

                        var variable = current.TryFindVariableCreation(identifer);
                        if (variable == null)
                        {
                            Console.WriteLine("null");
                            return null;
                        }

                        return variable;
                }
            }
        }*/
    }
}
