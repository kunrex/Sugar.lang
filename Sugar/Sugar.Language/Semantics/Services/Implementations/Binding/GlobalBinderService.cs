using System;

using Sugar.Language.Tokens.Enums;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Types;
using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Describers;
using Sugar.Language.Parsing.Nodes.UDDataTypes;
using Sugar.Language.Parsing.Nodes.Types.Enums;
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
using Sugar.Language.Semantics.Analysis.BuiltInTypes.Enums;
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
using Sugar.Language.Exceptions.Analytics.ClassMemberCreation.Casting;
using Sugar.Language.Exceptions.Analytics.ClassMemberCreation.Statements;
using Sugar.Language.Exceptions.Analytics.ClassMemberCreation.OperatorOverloads;

namespace Sugar.Language.Semantics.Services.Implementations.Binding
{
    internal sealed class GlobalBinderService : BinderService<IGlobalBinderService>
    {
        protected override string StepName { get => "Global Binding"; }

        public GlobalBinderService(DefaultNameSpaceNode _defaultNameSpace, CreatedNameSpaceCollectionNode _createdNameSpaces) : base(_defaultNameSpace, _createdNameSpaces)
        {
            
        }

        public override SemanticsResult Validate()
        {
            foreach (var type in defaultNameSpace.InternalDataTypes)
                CreateGlobalMembers(type);

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
            if (dataType.ActionNodeType == ActionNodeEnum.Enum)
                CreateEnumMembers((EnumType)dataType);
            else
            {
                var subTypeSearcher = new TypeSearcherService(dataType, defaultNameSpace, createdNameSpaces);

                switch (dataType.ActionNodeType)
                {
                    case ActionNodeEnum.Class:
                        CreateGlobalMembers<ClassType, ClassNode>((ClassType)dataType, subTypeSearcher, CreateGeneralMembers<ClassType, ClassNode>);
                        break;
                    case ActionNodeEnum.Struct:
                        CreateGlobalMembers<StructType, StructNode>((StructType)dataType, subTypeSearcher, CreateGeneralMembers<StructType, StructNode>);
                        break;
                    case ActionNodeEnum.Interface:
                        CreateGlobalMembers<InterfaceType, InterfaceNode>((InterfaceType)dataType, subTypeSearcher, CreateInterfaceMembers);
                        break;
                }
            }
        }

        private void CreateGlobalMembers<Type, BaseSkeleton>(Type dataType, TypeSearcherService subTypeSearcher, Action<Node, Type, TypeSearcherService> action) where Type : DataTypeWrapper<BaseSkeleton> where BaseSkeleton : UDDataTypeNode
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

        private void CreateGeneralMembers<Type, UDDTypeNode>(Node node, Type generalType, TypeSearcherService subTypeSearcher) where Type : DataTypeWrapper<UDDTypeNode>, IGeneralContainer where UDDTypeNode : UDDataTypeNode
        {
            var nodeType = node.NodeType;
            switch (nodeType)
            {
                case NodeType.Initialise:
                case NodeType.Declaration:
                    var entityCreationNode = (VariableCreationNode)node;

                    var name = (IdentifierNode)entityCreationNode.Name;
                    if (generalType.TryFindVariableCreation(name) != null)
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

                    AddOperatorOverload(generalType, operatorOverload, subTypeSearcher);
                    break;
                case NodeType.ExplicitDeclaration:
                    var explicitCast = (ExplicitCastDeclarationNode)node;

                    AddExplicitCastDelcration(generalType, explicitCast, generalType, subTypeSearcher, (TypeNode)explicitCast.Type);
                    break;
                case NodeType.ImplicitDeclaration:
                    var implicitCast = (ImplicitCastDeclarationNode)node;

                    AddImplicitCastDelcration(generalType, implicitCast, generalType, subTypeSearcher, (TypeNode)implicitCast.Type);
                    break;
                case NodeType.ConstructorDeclaration:
                    var constructor = (BaseFunctionDeclarationNode)node;

                    var functionInfo = GatherArguments(constructor, subTypeSearcher);
                    if (generalType.TryFindConstructorDeclaration(functionInfo.Arguments) != null)
                    {
                        result.Add(new DuplicateGlobalDefinitionException(generalType.Name, generalType.Name));
                        return;
                    }

                    AddDeclaration<ConstructorDeclarationStmt, IConstructorContainer>(new ConstructorDeclarationStmt(generalType, functionInfo.Describer, functionInfo.Arguments, functionInfo.Body), generalType);
                    break;
                case NodeType.MethodDeclaration:
                    var function = (FunctionDeclarationNode)node;

                    AddFunction(generalType, function, subTypeSearcher);
                    break;
                default:
                    result.Add(new InvalidStatementException("Invalid Statement", "Classes and Structs can only conatiner method entities, properties and variables"));
                    return;
            }
        }

        private void CreateInterfaceMembers(Node node, InterfaceType interfaceType, TypeSearcherService subTypeSearcher)
        {
            var nodeType = node.NodeType;
            switch (nodeType)
            {
                case NodeType.Initialise:
                case NodeType.Declaration:
                    var entityCreationNode = (VariableCreationNode)node;

                    var name = (IdentifierNode)entityCreationNode.Name;
                    if (interfaceType.TryFindPropertyCreation(name) != null)
                    {
                        result.Add(new DuplicateGlobalDefinitionException(name.Value, interfaceType.Name));
                    }

                    var type = (TypeNode)entityCreationNode.Type;
                    var describer = describerService.AnalyseDescriber((DescriberNode)entityCreationNode.Describer);

                    InitialisePropertyDeclaration(entityCreationNode, name, type, describer, interfaceType, subTypeSearcher);
                    break;
                case NodeType.Indexer:
                    var indexer = (IndexerDeclarationNode)node;

                    AddIndexer(indexer, interfaceType, subTypeSearcher, (CustomTypeNode)indexer.Type);
                    break;
                case NodeType.OperatorOverload:
                    var operatorOverload = (OperatorOverloadFunctionDeclarationNode)node;

                    AddOperatorOverload(interfaceType, operatorOverload, subTypeSearcher);
                    break;
                case NodeType.ExplicitDeclaration:
                    var explicitCast = (ExplicitCastDeclarationNode)node;

                    AddExplicitCastDelcration(interfaceType, explicitCast, interfaceType, subTypeSearcher, (TypeNode)explicitCast.Type);
                    break;
                case NodeType.ImplicitDeclaration:
                    var implicitCast = (ImplicitCastDeclarationNode)node;

                    AddImplicitCastDelcration(interfaceType, implicitCast, interfaceType, subTypeSearcher, (TypeNode)implicitCast.Type);
                    break;
                case NodeType.MethodDeclaration:
                    var function = (FunctionDeclarationNode)node;

                    AddFunction(interfaceType, function, subTypeSearcher);
                    break;
                default:
                    result.Add(new InvalidStatementException("Invalid Statement", "Interfaces can only conatiner method entities and properties"));
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

                        if (enumType.TryFindVariableCreation(name) != null)
                        {
                            result.Add(new DuplicateGlobalDefinitionException(name.Value, enumType.Name));
                            continue;
                        }

                        var type = defaultNameSpace.GetInternalDataType(TypeEnum.Integer);
                        InitailiseVariableDeclaration(entityCreationNode, name, type, describer, enumType);
                        break;
                    default:
                        result.Add(new InvalidStatementException("INvalid statement for enum.", "variable Creation Expcected"));
                        break;
                }
            }
        }

        private void InitailiseVariableDeclaration(VariableCreationNode node, IdentifierNode name, TypeNode type, Describer describer, IVariableContainer dataType, TypeSearcherService subTypeSearcher)
        {
            var result = FindReferencedType(subTypeSearcher, type);
            if (type == null)
                return;

            InitailiseVariableDeclaration(node, name, result, describer, dataType);
        }

        private void InitailiseVariableDeclaration(VariableCreationNode node, IdentifierNode name, DataType type, Describer describer, IVariableContainer dataType)
        {
            GlobalVariableDeclarationStmt declaration;
            if (node.NodeType == NodeType.Declaration)
                declaration = new GlobalVariableDeclarationStmt(type, name, describer);
            else
                declaration = new GlobalVariableInitialisationStmt(type, name, describer, ((InitializeNode)node).Value);

            AddDeclaration(declaration, dataType);
        }

        private void InitialisePropertyDeclaration(VariableCreationNode node, IdentifierNode name, TypeNode type, Describer describer, IPropertyContainer dataType, TypeSearcherService subTypeSearcher)
        {
            var referencedType = FindReferencedType(subTypeSearcher, type);
            if (referencedType == null)
                return;

            PropertyCreationStmt property;
            if (node.NodeType == NodeType.Declaration)
                property = InitialisePropertyDeclaration(node.Name, name, referencedType, describer);
            else
                property = InitialisePropertyDeclaration(node.Name, name, referencedType, describer, ((InitializeNode)node).Value);

            AddDeclaration(property, dataType);
        }

        private PropertyCreationStmt InitialisePropertyDeclaration(Node propertyNode, IdentifierNode name, DataType dataType, Describer describer, Node expressionValue = null)
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

        private void AddFunction<Type>(Type generalType, FunctionDeclarationNode function, TypeSearcherService subTypeSearcher) where Type : DataType, IFunctionContainer<MethodDeclarationStmt, VoidDeclarationStmt>
        {
            var name = (IdentifierNode)function.Name;
            var functionInfo = GatherArguments(function, subTypeSearcher, (TypeNode)function.Type);

            if (functionInfo.ReturnType == null)
            {
                if (generalType.TryFindVoidDeclaration(name, functionInfo.Arguments) != null)
                {
                    result.Add(new DuplicateGlobalDefinitionException(generalType.Name, generalType.Name));
                    return;
                }

                AddDeclaration(new VoidDeclarationStmt(name, functionInfo.Describer, functionInfo.Arguments, functionInfo.Body), generalType);
            }
            else
            {
                if (generalType.TryFindMethodDeclaration(name, functionInfo.Arguments) != null)
                {
                    result.Add(new DuplicateGlobalDefinitionException(generalType.Name, generalType.Name));
                    return;
                }

                AddDeclaration<MethodDeclarationStmt, IFunctionContainer<MethodDeclarationStmt, VoidDeclarationStmt>>(new MethodDeclarationStmt(functionInfo.ReturnType, name, functionInfo.Describer, functionInfo.Arguments, functionInfo.Body), generalType);
            }
        }

        private void AddOperatorOverload<Type>(Type generalType, OperatorOverloadFunctionDeclarationNode operatorOverload, TypeSearcherService subTypeSearcher) where Type : DataType, IOperatorContainer
        {
            var functionInfo = GatherArguments(operatorOverload, subTypeSearcher, (TypeNode)operatorOverload.Type);
            switch (operatorOverload.Operator.Type)
            {
                case TokenType.UnaryOperator:
                    if (functionInfo.Arguments.Count != 1)
                    {
                        result.Add(new OperatorArgumentCounMismatchException(operatorOverload.Operator.Type));
                        return;
                    }
                    else if (generalType.TryFindOperatorOverloadDeclaration(operatorOverload.Operator, functionInfo.Arguments[0]) != null)
                    {
                        result.Add(new DuplicateGlobalDefinitionException(operatorOverload.Operator.Value, generalType.Name));
                        return;
                    }
                    break;
                case TokenType.BinaryOperator:
                    if (functionInfo.Arguments.Count != 2)
                        result.Add(new OperatorArgumentCounMismatchException(operatorOverload.Operator.Type));

                    var found = false;
                    for (int i = 0; i < functionInfo.Arguments.Count; i++)
                        if (functionInfo.Arguments[i] == generalType)
                        {
                            found = true;
                            break;
                        }

                    if (!found)
                    {
                        result.Add(new BinaryOperationArgumentException(0));
                        return;
                    }
                    else if (generalType.TryFindOperatorOverloadDeclaration(operatorOverload.Operator, functionInfo.Arguments[0], functionInfo.Arguments[1]) != null)
                    {
                        result.Add(new DuplicateGlobalDefinitionException(operatorOverload.Operator.Value, generalType.Name));
                        return;
                    }
                    break;
            }

            AddDeclaration<OperatorOverloadDeclarationStmt, IOperatorContainer>(new OperatorOverloadDeclarationStmt(functionInfo.ReturnType, functionInfo.Describer, functionInfo.Arguments, functionInfo.Body, operatorOverload.Operator), generalType);
        }

        private void AddIndexer(IndexerDeclarationNode baseFunction, IIndexerContainer indexerContainer, TypeSearcherService subTypeSearcher, TypeNode type)
        {
            var returnType = FindReferencedType(subTypeSearcher, type);
            var functionInfo = GatherArguments(baseFunction, subTypeSearcher);

            if (indexerContainer.TryFindIndexerCreationStatement(returnType, functionInfo.Arguments) != null)
                return;

            var body = baseFunction.Body;
            switch (baseFunction.Body.NodeType)
            {
                case NodeType.Get:
                    var getNode = (PropertyGetNode)body;
                    AddDeclaration(new IndexerCreationStmt(returnType, functionInfo.Describer, functionInfo.Arguments, getNode.Get, null), indexerContainer);
                    break;
                case NodeType.Set:
                    var setNode = (PropertySetNode)body;
                    AddDeclaration(new IndexerCreationStmt(returnType, functionInfo.Describer, functionInfo.Arguments, null, setNode.Set), indexerContainer);
                    break;
                default:
                    var getSetNode = (PropertyGetSetNode)body;
                    AddDeclaration(new IndexerCreationStmt(returnType, functionInfo.Describer, functionInfo.Arguments, getSetNode.Get, getSetNode.Set), indexerContainer);
                    break;
            }
        }

        private void AddExplicitCastDelcration(DataType parent, ExplicitCastDeclarationNode baseFunction, IExplicitContainer explicitContainer, TypeSearcherService subTypeSearcher, TypeNode type)
        {
            var returnType = FindReferencedType(subTypeSearcher, type);

            if (returnType == null)
                return;
            if (explicitContainer.TryFindExplicitCastDeclaration(returnType) != null)
                return;

            var functionInfo = GatherArguments(baseFunction, subTypeSearcher);
            if (functionInfo.Arguments.Count != 1)
                result.Add(new CastDeclarationArgumentsException(0));
            else
            {
                var arg = functionInfo.Arguments[0];
                if ((arg != parent && returnType != parent) || arg == returnType)
                    result.Add(new CastDeclarationException(0));
                else
                    AddDeclaration<ExplicitCastDeclarationStmt, ICastContainer<ExplicitCastDeclarationStmt, IExplicitContainer>>(new ExplicitCastDeclarationStmt(returnType, functionInfo.Describer, functionInfo.Arguments, functionInfo.Body), explicitContainer);
            }
        }

        private void AddImplicitCastDelcration(DataType parent, ImplicitCastDeclarationNode baseFunction, IImplicitContainer implicitContainer, TypeSearcherService subTypeSearcher, TypeNode type)
        {
            var returnType = FindReferencedType(subTypeSearcher, type);

            if (returnType == null)
                return;
            if (implicitContainer.TryFindImplicitCastDeclaration(returnType) != null)
                return;

            var functionInfo = GatherArguments(baseFunction, subTypeSearcher);
            if (functionInfo.Arguments.Count != 1)
                result.Add(new CastDeclarationArgumentsException(0));
            else
            {
                var arg = functionInfo.Arguments[0];
                if ((arg != parent && returnType != parent) || arg == returnType)
                    result.Add(new CastDeclarationException(0));
                else
                    AddDeclaration<ImplicitCastDeclarationStmt, ICastContainer<ImplicitCastDeclarationStmt, IImplicitContainer>>(new ImplicitCastDeclarationStmt(returnType, functionInfo.Describer, functionInfo.Arguments, functionInfo.Body), implicitContainer);
            }
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

        private FunctionInfo GatherArguments(BaseFunctionDeclarationNode baseFunction, TypeSearcherService subTypeSearcher)
        {
            return new FunctionInfo(baseFunction.Body,
                                    null,
                                    describerService.AnalyseDescriber((DescriberNode)baseFunction.Describer),
                                    CreateFunctionArguments(subTypeSearcher, (FunctionDeclarationArgumentsNode)baseFunction.Arguments));
        }

        private FunctionInfo GatherArguments(BaseFunctionDeclarationNode baseFunction, TypeSearcherService subTypeSearcher, TypeNode type)
        {
            var returnType = type.Type == TypeNodeEnum.Void ? null : FindReferencedType(subTypeSearcher, type);

            return new FunctionInfo(baseFunction.Body,
                                    returnType,
                                    describerService.AnalyseDescriber((DescriberNode)baseFunction.Describer),
                                    CreateFunctionArguments(subTypeSearcher, (FunctionDeclarationArgumentsNode)baseFunction.Arguments));
        }

        private FunctionDeclArgs CreateFunctionArguments(TypeSearcherService subTypeSearcher, FunctionDeclarationArgumentsNode argumentsNode)
        {
            var arguments = new FunctionDeclArgs();

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

                var type = FindReferencedType(subTypeSearcher, (TypeNode)child.Type);
                if (type == null)
                    continue;

                if (child.DefaultValue == null)
                    argument = new FunctionArgumentDeclarationStmt(type, name, describer);
                else
                    argument = new FunctionArgumentInitialisationStmt(type, name, describer, child.DefaultValue);

                arguments.Add(name.Value, argument);
            }

            return arguments;
        }
    }
}
