using System;

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
using Sugar.Language.Exceptions.Analytics.ClassMemberCreation.Statements;
using Sugar.Language.Semantics.Analysis.BuiltInTypes.Enums;
using Sugar.Language.Parsing.Nodes.Types;
using Sugar.Language.Parsing.Nodes.Types.Enums;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions;

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
            var integer = defaultNameSpace.GetInternalDataType(TypeEnum.Integer);
            if (integer != null)
            {
                CreateGlobalMembers(integer);
            }

            //CreateCollectionGlobalMembers(defaultNameSpace);
            //CreateCollectionGlobalMembers(createdNameSpaces);

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
                        CreateGlobalMembers<ClassType, ClassNode>((ClassType)dataType, subTypeSearcher, CreateGeneralMembers<ClassType, ClassNode>);
                        break;
                    case DataTypeEnum.Struct:
                        CreateGlobalMembers<StructType, StructNode>((StructType)dataType, subTypeSearcher, CreateGeneralMembers<StructType, StructNode>);
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
                    {
                        action.Invoke(child, dataType, subTypeSearcher);
                                          Console.WriteLine("hi");
                    }
                    break;
                default:
                    action.Invoke(body, dataType, subTypeSearcher);
                    break;
            }
        }

        //match type nodes, like how contructor nodes have their own class, check for those
        private void CreateGeneralMembers<Type, UDDTypeNode>(Node node, Type generalType, SubTypeSearcherService subTypeSearcher) where Type : DataTypeWrapper<UDDTypeNode>, IGeneralContainer where UDDTypeNode : UDDataTypeNode
        {
            var nodeType = node.NodeType;
            switch (nodeType)
            {
                case NodeType.Initialise:
                case NodeType.Declaration:
                    var entityCreationNode = (VariableCreationNode)node;

                    var name = (IdentifierNode)entityCreationNode.Name;
                    if(generalType.IsDuplicate(name))
                    {
                        result.Add(new DuplicateGlobalDefinitionException(name.Value, generalType.Name));
                        return;
                    }

                    var type = (TypeNode)entityCreationNode.Type;
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
                    var indexer = (IndexerDeclarationNode)node;

                    AddIndexer(indexer, generalType, subTypeSearcher, (CustomTypeNode)indexer.Type);
                    break;
                case NodeType.OperatorOverload:
                    var operatorOverload = (OperatorOverloadFunctionDeclarationNode)node;

                    var functionInfo = GatherArguments(operatorOverload, subTypeSearcher);
                    AddOperatorOverload(operatorOverload, functionInfo.Name, generalType, functionInfo.Body, functionInfo.ReturnType, functionInfo.Describer, functionInfo.Arguments);
                    break;
                case NodeType.ExplicitDeclaration:
                    var explicitCast = (ExplicitCastDeclarationNode)node;

                    functionInfo = GatherArguments(explicitCast, subTypeSearcher);
                    AddDeclaration<ExplicitCastDeclarationStmt, ICastContainer<ExplicitCastDeclarationStmt, IExplicitContainer>>(new ExplicitCastDeclarationStmt(functionInfo.ReturnType, functionInfo.Name, functionInfo.Describer, functionInfo.Arguments, functionInfo.Body), generalType);
                    break;
                case NodeType.ImplicitDeclaration:
                    var implicitCast = (ImplicitCastDeclarationNode)node;

                    functionInfo = GatherArguments(implicitCast, subTypeSearcher);
                    AddDeclaration<ImplicitCastDeclarationStmt, ICastContainer<ImplicitCastDeclarationStmt, IImplicitContainer>>(new ImplicitCastDeclarationStmt(functionInfo.ReturnType, functionInfo.Name, functionInfo.Describer, functionInfo.Arguments, functionInfo.Body), generalType);
                    break;
                case NodeType.ConstructorDeclaration:
                    var constructor = (BaseFunctionDeclarationNode)node;

                    functionInfo = GatherArguments(constructor, subTypeSearcher);
                    AddDeclaration<ConstructorDeclarationStmt, IConstructorContainer>(new ConstructorDeclarationStmt(generalType, functionInfo.Name, functionInfo.Describer, functionInfo.Arguments, functionInfo.Body), generalType);
                    break;
                case NodeType.FunctionDeclaration:
                    var function = (FunctionDeclarationNode)node;

                    functionInfo = GatherArguments(function, subTypeSearcher);
                    if (functionInfo.ReturnType == null)
                        AddDeclaration(new VoidDeclarationStmt(functionInfo.Name, functionInfo.Describer, functionInfo.Arguments, functionInfo.Body), generalType);
                    else
                        AddDeclaration<MethodDeclarationStmt, IFunctionContainer<MethodDeclarationStmt, VoidDeclarationStmt>>(new MethodDeclarationStmt(functionInfo.ReturnType, functionInfo.Name, functionInfo.Describer, functionInfo.Arguments, functionInfo.Body), generalType);

                    break;
                default:
                    Console.WriteLine("Invalid Statement");
                    return;
            }
        }

        private void CreateInterfaceMembers(Node node, InterfaceType interfaceType, SubTypeSearcherService subTypeSearcher)
        {
            var nodeType = node.NodeType;
            switch (nodeType)
            {
                case NodeType.Initialise:
                case NodeType.Declaration:
                    var entityCreationNode = (VariableCreationNode)node;

                    var name = (IdentifierNode)entityCreationNode.Name;
                    if(interfaceType.IsDuplicate(name))
                    {
                        result.Add(new DuplicateGlobalDefinitionException(name.Value, interfaceType.Name));
                    }

                    var type = (TypeNode)entityCreationNode.Type;
                    var describer = describerService.AnalyseDescriber((DescriberNode)entityCreationNode.Describer);

                    InitialisePropertyDeclaration(entityCreationNode, name, type, describer, interfaceType, subTypeSearcher);
                    break;
                case NodeType.Indexer:
                    var creationNode = (BaseFunctionDeclarationNode)node;

                    AddIndexer(creationNode, interfaceType, subTypeSearcher, (CustomTypeNode)creationNode.Type);
                    break;
                case NodeType.OperatorOverload:
                case NodeType.ExplicitDeclaration:
                case NodeType.FunctionDeclaration:
                case NodeType.ImplicitDeclaration:
                    creationNode = (BaseFunctionDeclarationNode)node;

                    FunctionInfo functionInfo;
                    if (nodeType == NodeType.ConstructorDeclaration)
                        functionInfo = GatherArguments(creationNode, subTypeSearcher);
                    else
                        functionInfo = GatherArguments(creationNode, subTypeSearcher, (TypeNode)creationNode.Type);

                    if (interfaceType.IsDuplicate(functionInfo.Name))
                    {
                        result.Add(new DuplicateGlobalDefinitionException(functionInfo.Name.Value, interfaceType.Name));
                    }

                    switch (nodeType)
                    {
                        case NodeType.FunctionDeclaration:
                            if (functionInfo.ReturnType == null)
                                AddDeclaration(new VoidDeclarationStmt(functionInfo.Name, functionInfo.Describer, functionInfo.Arguments, functionInfo.Body), interfaceType);
                            else
                                AddDeclaration<MethodDeclarationStmt, IFunctionContainer<MethodDeclarationStmt, VoidDeclarationStmt>>(new MethodDeclarationStmt(functionInfo.ReturnType, functionInfo.Name, functionInfo.Describer, functionInfo.Arguments, functionInfo.Body), interfaceType);
                            break;
                        case NodeType.ImplicitDeclaration:
                            AddDeclaration<ImplicitCastDeclarationStmt, ICastContainer<ImplicitCastDeclarationStmt, IImplicitContainer>>(new ImplicitCastDeclarationStmt(functionInfo.ReturnType, functionInfo.Name, functionInfo.Describer, functionInfo.Arguments, functionInfo.Body), interfaceType);
                            break;
                        case NodeType.ExplicitDeclaration:
                            AddDeclaration<ExplicitCastDeclarationStmt, ICastContainer<ExplicitCastDeclarationStmt, IExplicitContainer>>(new ExplicitCastDeclarationStmt(functionInfo.ReturnType, functionInfo.Name, functionInfo.Describer, functionInfo.Arguments, functionInfo.Body), interfaceType);
                            break;
                        case NodeType.OperatorOverload:
                            AddOperatorOverload(creationNode, functionInfo.Name, interfaceType, functionInfo.Body, functionInfo.ReturnType, functionInfo.Describer, functionInfo.Arguments);
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
                    case NodeType.Assignment:
                        var entityCreationNode = (VariableCreationNode)child;

                        var describer = new Describer(DescriberEnum.EnumModifiers);
                        var name = (IdentifierNode)entityCreationNode.Name;
                        var type = defaultNameSpace.GetInternalDataType(TypeEnum.Integer);

                        InitailiseVariableDeclaration(entityCreationNode, name, type, describer, enumType);
                        break;
                    default:
                        Console.WriteLine("Invalid Statement");
                        return;
                }
            }
        }

        private void InitailiseVariableDeclaration(VariableCreationNode node, IdentifierNode name, TypeNode type, Describer describer, IVariableContainer dataType, SubTypeSearcherService subTypeSearcher)
            => InitailiseVariableDeclaration(node, name, FindReferencedType(subTypeSearcher, type), describer, dataType);

        private void InitailiseVariableDeclaration(VariableCreationNode node, IdentifierNode name, DataType type, Describer describer, IVariableContainer dataType)
        {
            VariableCreationStmt variableCreationStatement;
            if (node.NodeType == NodeType.Declaration)
                variableCreationStatement = new GlobalVariableDeclarationStmt(type, name, describer);
            else
                variableCreationStatement = new GlobalVariableInitialisationStmt(type, name, describer, (ExpressionNode)((InitializeNode)node).Value);

            AddDeclaration(variableCreationStatement, dataType);
        }

        private void InitialisePropertyDeclaration(VariableCreationNode node, IdentifierNode name, TypeNode type, Describer describer, IPropertyContainer dataType, SubTypeSearcherService subTypeSearcher)
        {
            var referencedType = FindReferencedType(subTypeSearcher, type);

            PropertyCreationStmt property;
            if (node.NodeType == NodeType.Declaration)
                property = InitialisePropertyDeclaration(node.Name, name, referencedType, describer);
            else
                property = InitialisePropertyDeclaration(node.Name, name, referencedType, describer, (ExpressionNode)((InitializeNode)node).Value);

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
        //spilt GathrArguments based on the function type and not generalise it
        //conversions should also check for built in types and all that
        private FunctionInfo GatherArguments(BaseFunctionDeclarationNode baseFunction, SubTypeSearcherService subTypeSearcher, TypeNode type) => new FunctionInfo(baseFunction.Body,
                                            (IdentifierNode) baseFunction.Name,
                                            FindReferencedType(subTypeSearcher, type),
                                            describerService.AnalyseDescriber((DescriberNode) baseFunction.Describer),
                                            CreateArguments(subTypeSearcher, (FunctionDeclarationArgumentsNode) baseFunction.Arguments));

        private DataType FindReferencedType(SubTypeSearcherService subTypeSearcher, TypeNode type)
        {
            switch(type.Type)
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

        //split GatherArguments per function
        //change the Name system of functions, not all functions are named by IdentifierNodes, some use Operators and TypeKeywords
        private FunctionInfo GatherArguments(OperatorOverloadDeclarationStmt baseFunction, SubTypeSearcherService subTypeSearcher)
        {
            var name = baseFunction.Operator;

            //return new FunctionInfo(baseFunction.NodeBody, )
            return new FunctionInfo();
        }

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

        private void AddDeclaration<CreationStatementType, Container>(CreationStatementType declaration, Container container) where CreationStatementType : ICreationStatement where Container : IContainer<CreationStatementType, Container>
        {
            if (!declaration.ValidateDescriber())
                result.Add(new InvalidDescriberException(declaration.Name, declaration.Allowed, declaration.Describer));
            else
                container.AddDeclaration(declaration);
        }

        private void AddDeclaration(VoidDeclarationStmt declaration, IFunctionContainer<MethodDeclarationStmt, VoidDeclarationStmt> container) 
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
                    argument = new FunctionArgumentDeclarationStmt(FindReferencedType(subTypeSearcher, (TypeNode)child.Type), name, describer);
                else
                    argument = new FunctionArgumentInitialisationStmt(FindReferencedType(subTypeSearcher, (TypeNode)child.Type), name, describer, (ExpressionNode)child.DefaultValue);

                arguments.Add(name.Value, argument);
            }

            return arguments;
        }
    }
}
