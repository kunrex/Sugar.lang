﻿using System;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Describers;
using Sugar.Language.Parsing.Nodes.Expressions;
using Sugar.Language.Parsing.Nodes.UDDataTypes;
using Sugar.Language.Parsing.Nodes.Types.Subtypes;
using Sugar.Language.Parsing.Nodes.Functions.Properties;
using Sugar.Language.Parsing.Nodes.Functions.Declarations;
using Sugar.Language.Parsing.Nodes.Statements.VariableCreation;
using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;
using Sugar.Language.Parsing.Nodes.Functions.Declarations.OperatorOverloading;

using Sugar.Language.Semantics.Analysis;
using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.Services.Interfaces;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.Namespaces;
using Sugar.Language.Semantics.ActionTrees.CreationStatements;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Interfaces.Collections;
using Sugar.Language.Semantics.Services.Implementations.Structures;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes.Casts;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.PropertyCreation;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global.Conversion;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation.Local.FunctionArguments;

using Sugar.Language.Exceptions.Analytics.ClassMemberCreation;
using Sugar.Language.Parsing.Nodes.NodeGroups;

namespace Sugar.Language.Semantics.Services.Implementations
{
    internal sealed class ClassMemberService : IClassMemeberService
    {
        private readonly DefaultNameSpaceNode defaultNameSpace;
        private readonly CreatedNameSpaceCollectionNode createdNameSpaces;

        private readonly DescriberService describerService;

        private readonly SemanticsResult result;

        public ClassMemberService(DefaultNameSpaceNode _defaultNameSpace, CreatedNameSpaceCollectionNode _createdNameSpaces)
        {
            defaultNameSpace = _defaultNameSpace;
            createdNameSpaces = _createdNameSpaces;

            describerService = new DescriberService();

            result = new SemanticsResult("Class Member Creation");
        }

        public SemanticsResult Validate()
        {
            CreateCollectionGlobalMembers(defaultNameSpace);
            CreateCollectionGlobalMembers(createdNameSpaces);

            return result.Build();
        }

        private void CreateCollectionGlobalMembers(IDataTypeCollection collection)
        {
            for (int i = 0; i < collection.DataTypeCount; i++)
                CreateGlobalMembers(collection.GetSubDataType(i));
        }

        private void CreateCollectionGlobalMembers(INameSpaceCollection collection)
        {
            for (int i = 0; i < collection.NameSpaceCount; i++)
                CreateGlobalMembers(collection.GetSubNameSpace(i));
        }

        private void CreateGlobalMembers(CreatedNameSpaceNode nameSpace)
        {
            int i;
            for (i = 0; i < nameSpace.DataTypeCount; i++)
                CreateGlobalMembers(nameSpace.GetSubDataType(i));

            for (i = 0; i < nameSpace.NameSpaceCount; i++)
                CreateGlobalMembers(nameSpace.GetSubNameSpace(i));
        }

        private void CreateGlobalMembers(DataType dataType)
        {
            if(dataType.TypeEnum == DataTypeEnum.Enum)
                CreateEnumMembers((EnumType)dataType);
            else
            {
                var subTypeSearcher = new SubTypeSearcherService(dataType, defaultNameSpace, createdNameSpaces);

                switch(dataType.TypeEnum)
                {
                    case DataTypeEnum.Class:
                        CreateGlobalMembers<ClassType, ClassNode>((ClassType)dataType, subTypeSearcher, CreateGeneralMembers);
                        break;
                    case DataTypeEnum.Struct:
                        CreateGlobalMembers<StructType, StructNode>((StructType)dataType, subTypeSearcher, CreateGeneralMembers);
                        break;
                    case DataTypeEnum.Interface:
                        CreateGlobalMembers<InterfaceType, InterfaceNode>((InterfaceType)dataType, subTypeSearcher, CreateInterfaceMembers);
                        break;
                }
            }
        }

        private void CreateGlobalMembers<Type, BaseSkeleton>(Type dataType, SubTypeSearcherService subTypeSearcher, Action<Node, Type, SubTypeSearcherService> action) where Type : DataTypeWrapper<BaseSkeleton> where BaseSkeleton : UDDataTypeNode
        {
            Node body = dataType.Skeleton.Body;

            switch (body.NodeType)
            {
                case NodeType.Scope:
                    foreach (var child in body.GetChildren())
                        action.Invoke(child, dataType, subTypeSearcher);
                    break;
                default:
                    action.Invoke(body, dataType, subTypeSearcher);
                    break;
            }
        }

        private void CreateGeneralMembers<Type>(Node node, Type generalType, SubTypeSearcherService subTypeSearcher) where Type : DataType, IGeneralContainer
        {
            var nodeType = node.NodeType;
            switch (nodeType)
            {
                case NodeType.Initialise:
                case NodeType.Declaration:
                    var entityCreationNode = (VariableCreationNode)node;

                    var type = entityCreationNode.Type;
                    var name = (IdentifierNode)entityCreationNode.Name;
                    var describer = describerService.AnalyseDescriber((DescriberNode)entityCreationNode.Describer);

                    switch (entityCreationNode.CreationType)
                    {
                        case NodeType.Variable:
                            InitailiseVariableDeclaration(entityCreationNode, name, type, describer, generalType, subTypeSearcher);
                            break;
                        default:
                            InitialisePropertyDeclaration(entityCreationNode, name, type, describer, generalType, subTypeSearcher);
                            break;
                    }
                    break;
                case NodeType.Indexer:
                    var creationNode = (BaseFunctionDeclarationNode)node;

                    AddIndexer(creationNode, generalType, subTypeSearcher, (CustomTypeNode)creationNode.Type);
                    break;
                case NodeType.OperatorOverload:
                case NodeType.ExplicitDeclaration:
                case NodeType.FunctionDeclaration:
                case NodeType.ImplicitDeclaration:
                case NodeType.ConstructorDeclaration:
                    creationNode = (BaseFunctionDeclarationNode)node;

                    FunctionInfo result;
                    if(nodeType == NodeType.ConstructorDeclaration)
                        result = GatherArguments(creationNode, subTypeSearcher);
                    else
                        result = GatherArguments(creationNode, subTypeSearcher, (CustomTypeNode)creationNode.Type);

                    switch(nodeType)
                    {
                        case NodeType.FunctionDeclaration:
                            AddDeclaration<MethodDeclarationStmt, IFunctionContainer<MethodDeclarationStmt>>(new MethodDeclarationStmt(result.ReturnType, result.Name, result.Describer, result.Arguments, result.Body), generalType);
                            break;
                        case NodeType.ConstructorDeclaration:
                            AddDeclaration<ConstructorDeclarationStmt, IConstructorContainer>(new ConstructorDeclarationStmt(generalType, result.Name, result.Describer, result.Arguments, result.Body), generalType);
                            break;
                        case NodeType.ImplicitDeclaration:
                            AddDeclaration<ImplicitCastDeclarationStmt, ICastContainer<ImplicitCastDeclarationStmt, IImplicitContainer>>(new ImplicitCastDeclarationStmt(result.ReturnType, result.Name, result.Describer, result.Arguments, result.Body), generalType);
                            break;
                        case NodeType.ExplicitDeclaration:
                            AddDeclaration<ExplicitCastDeclarationStmt, ICastContainer<ExplicitCastDeclarationStmt, IExplicitContainer>>(new ExplicitCastDeclarationStmt(result.ReturnType, result.Name, result.Describer, result.Arguments, result.Body), generalType);
                            break;
                        case NodeType.OperatorOverload:
                            AddOperatorOverload(creationNode, result.Name, generalType, result.Body, result.ReturnType, result.Describer, result.Arguments);
                            break;
                    }
                    break;
                default:
                    Console.WriteLine("Invalid Statement");
                    return;
            }
        }

        private void CreateInterfaceMembers(Node node, InterfaceType classType, SubTypeSearcherService subTypeSearcher)
        {
            var nodeType = node.NodeType;
            switch (nodeType)
            {
                case NodeType.Initialise:
                case NodeType.Declaration:
                    var entityCreationNode = (VariableCreationNode)node;

                    var type = entityCreationNode.Type;
                    var name = (IdentifierNode)entityCreationNode.Name;
                    var describer = describerService.AnalyseDescriber((DescriberNode)entityCreationNode.Describer);

                    InitialisePropertyDeclaration(entityCreationNode, name, type, describer, classType, subTypeSearcher);
                    break;
                case NodeType.Indexer:
                    var creationNode = (BaseFunctionDeclarationNode)node;

                    AddIndexer(creationNode, classType, subTypeSearcher, (CustomTypeNode)creationNode.Type);
                    break;
                case NodeType.OperatorOverload:
                case NodeType.ExplicitDeclaration:
                case NodeType.FunctionDeclaration:
                case NodeType.ImplicitDeclaration:
                    creationNode = (BaseFunctionDeclarationNode)node;

                    FunctionInfo result;
                    if (nodeType == NodeType.ConstructorDeclaration)
                        result = GatherArguments(creationNode, subTypeSearcher);
                    else
                        result = GatherArguments(creationNode, subTypeSearcher, (CustomTypeNode)creationNode.Type);

                    switch (nodeType)
                    {
                        case NodeType.FunctionDeclaration:
                            AddDeclaration<MethodDeclarationStmt, IFunctionContainer<MethodDeclarationStmt>>(new MethodDeclarationStmt(result.ReturnType, result.Name, result.Describer, result.Arguments, result.Body), classType);
                            break;
                        case NodeType.ImplicitDeclaration:
                            AddDeclaration<ImplicitCastDeclarationStmt, ICastContainer<ImplicitCastDeclarationStmt, IImplicitContainer>>(new ImplicitCastDeclarationStmt(result.ReturnType, result.Name, result.Describer, result.Arguments, result.Body), classType);
                            break;
                        case NodeType.ExplicitDeclaration:
                            AddDeclaration<ExplicitCastDeclarationStmt, ICastContainer<ExplicitCastDeclarationStmt, IExplicitContainer>>(new ExplicitCastDeclarationStmt(result.ReturnType, result.Name, result.Describer, result.Arguments, result.Body), classType);
                            break;
                        case NodeType.OperatorOverload:
                            AddOperatorOverload(creationNode, result.Name, classType, result.Body, result.ReturnType, result.Describer, result.Arguments);
                            break;
                    }
                    break;
                default:
                    Console.WriteLine("Invalid Statement");
                    return;
            }
        }

        private void CreateEnumMembers(EnumType enumType)
        {
            Node body = enumType.Skeleton.Body;

            foreach (var child in body.GetChildren())
            {
                switch (child.NodeType)
                {
                    case NodeType.Variable:

                        break;
                    case NodeType.Assignment:
                        break;
                    default:
                        Console.WriteLine("Invalid Statement");
                        return;
                }
            }
        }

        private void InitailiseVariableDeclaration(VariableCreationNode node, IdentifierNode name, Node type, Describer describer, IVariableContainer dataType, SubTypeSearcherService subTypeSearcher) 
        {
            VariableCreationStmt variableCreationStatement;
            if (node.NodeType == NodeType.Declaration)
                variableCreationStatement = new GlobalVariableDeclarationStmt(subTypeSearcher.TryFindReferencedType(type), name, describer);
            else
                variableCreationStatement = new GlobalVariableInitialisationStmt(subTypeSearcher.TryFindReferencedType(type), name, describer, (ExpressionNode)((InitializeNode)node).Value);

            AddDeclaration(variableCreationStatement, dataType);
        }

        private void InitialisePropertyDeclaration(VariableCreationNode node, IdentifierNode name, Node type, Describer describer, IPropertyContainer dataType, SubTypeSearcherService subTypeSearcher)
        {
            PropertyCreationStmt property;
            if (node.NodeType == NodeType.Declaration)
                property = InitialisePropertyDeclaration(node.Name, name, subTypeSearcher.TryFindReferencedType(type), describer);
            else
                property = InitialisePropertyDeclaration(node.Name, name, subTypeSearcher.TryFindReferencedType(type), describer, (ExpressionNode)((InitializeNode)node).Value);

            AddDeclaration(property, dataType);
        }

        private PropertyCreationStmt InitialisePropertyDeclaration(Node propertyNode, IdentifierNode name, DataType dataType, Describer describer, ExpressionNode expressionValue = null)
        {
            PropertyCreationStmt propertyCreationNode;

            switch (propertyNode.NodeType)
            {
                case NodeType.PropertyGet:
                    var getNode = (PropertyGetNode)propertyNode;
                    propertyCreationNode = CreateStatement(getNode.Get, null);
                    break;
                case NodeType.PropertySet:
                    var setNode = (PropertySetNode)propertyNode;
                    propertyCreationNode = CreateStatement(null, setNode.Set);
                    break;
                default:
                    var getSetNode = (PropertyGetSetNode)propertyNode;
                    propertyCreationNode = CreateStatement(getSetNode.Get, getSetNode.Set);
                    break;
            }

            if (!propertyCreationNode.ValidateDescriber())
            {
                result.Add(new InvalidDescriberException(name.Value, propertyCreationNode.Allowed, propertyCreationNode.Describer));
                return null;
            }
            else
                return propertyCreationNode;

            PropertyCreationStmt CreateStatement(Node get, Node set)
            {
                if (expressionValue == null)
                    return new PropertyDeclarationStmt(dataType, name, describer, get, set);
                else
                    return new PropertyInitialisationStmt(dataType, name, describer, get, set, expressionValue);
            }
        }

        private FunctionInfo GatherArguments(BaseFunctionDeclarationNode baseFunction, SubTypeSearcherService subTypeSearcher, CustomTypeNode type) => new FunctionInfo(
                baseFunction.Body,
                (IdentifierNode)baseFunction.Name,
                subTypeSearcher.TryFindReferencedType(type.CustomType),
                describerService.AnalyseDescriber((DescriberNode)baseFunction.Describer),
                CreateArguments(subTypeSearcher, (FunctionDeclarationArgumentsNode)baseFunction.Arguments));

        private FunctionInfo GatherArguments(BaseFunctionDeclarationNode baseFunction, SubTypeSearcherService subTypeSearcher) => new FunctionInfo(
                baseFunction.Body,
                (IdentifierNode)baseFunction.Name,
                null,
                describerService.AnalyseDescriber((DescriberNode)baseFunction.Describer),
                CreateArguments(subTypeSearcher, (FunctionDeclarationArgumentsNode)baseFunction.Arguments));

        private void AddIndexer(BaseFunctionDeclarationNode baseFunction, IIndexerContainer indexerContainer, SubTypeSearcherService subTypeSearcher, CustomTypeNode type)
        {
            var returnType = subTypeSearcher.TryFindReferencedType(type.CustomType);

            if (indexerContainer.IsDuplicateIndexer(returnType))
            {
                Console.WriteLine("duplicate");
                return;
            }

            var arguments = CreateArguments(subTypeSearcher, (FunctionDeclarationArgumentsNode)baseFunction.Arguments);
            var describer = describerService.AnalyseDescriber((DescriberNode)baseFunction.Describer);

            var indexerNode = (IndexerDeclarationNode)baseFunction;
            var property = InitialisePropertyDeclaration(indexerNode.Body, returnType.Identifier, returnType, describer);

            AddDeclaration(new IndexerCreationStmt(returnType, null, describer, arguments, property), indexerContainer);
        }

        private void AddOperatorOverload(BaseFunctionDeclarationNode baseFunction, IdentifierNode name, IOperatorContainer operatorContainer, Node body, DataType returnType, Describer describer, FunctionArguments arguments)
        {
            var overloadOperator = ((OperatorOverloadFunctionDeclarationNode)baseFunction).Operator;

            AddDeclaration(new OperatorOverloadDeclarationStmt(returnType, name, describer, arguments, body, overloadOperator), operatorContainer);
        }

        private void AddDeclaration<CreationStatementType, Container>(CreationStatementType declaration,  Container container) where CreationStatementType : ICreationStatement where Container : IContainer<CreationStatementType, Container>
        {
            if (!declaration.ValidateDescriber())
                result.Add(new InvalidDescriberException(declaration.Name, declaration.Allowed, declaration.Describer));
            else
                container.AddDeclaration(declaration);
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

                if (child.DefaultValue == null)
                    argument = new FunctionArgumentDeclarationStmt(subTypeSearcher.TryFindReferencedType(child.Type), name, describer);
                else
                    argument = new FunctionArgumentInitialisationStmt(subTypeSearcher.TryFindReferencedType(child.Type), name, describer, (ExpressionNode)child.DefaultValue);

                arguments.Add(name.Value, argument);
            }

            return arguments;
        }
    }
}
