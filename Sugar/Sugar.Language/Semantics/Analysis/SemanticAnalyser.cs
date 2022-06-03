using System;
using System.Collections.Generic;

using Sugar.Language.Tokens.Enums;

using Sugar.Language.Parsing;
using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.UDDataTypes;
using Sugar.Language.Parsing.Nodes.Expressions.Associative;

using Sugar.Language.Semantics.ActionTrees;
using Sugar.Language.Semantics.ActionTrees.Namespaces;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Interfaces.Collections;
using Sugar.Language.Parsing.Nodes.Statements;
using Sugar.Language.Parsing.Nodes.UDDataTypes.Enums;
using Sugar.Language.Semantics.Analysis.Structure;
using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Parsing.Nodes.Statements.VariableCreation;
using Sugar.Language.Parsing.Nodes.Types.Subtypes;
using Sugar.Language.Parsing.Nodes.Describers;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation;
using Sugar.Language.Parsing.Nodes.Expressions;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.PropertyCreation;
using Sugar.Language.Parsing.Nodes.Functions.Properties;
using Sugar.Language.Parsing.Nodes.Functions.Declarations;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global;
using Sugar.Language.Semantics.ActionTrees.CreationStatements;
using Sugar.Language.Semantics.ActionTrees.Interfaces;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;
using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;
using Sugar.Language.Semantics.Services.Implementations;

namespace Sugar.Language.Semantics.Analysis
{
    internal sealed class SemanticAnalyser
    {
        private SugarPackage package;
        public SyntaxTreeCollection Collection { get; private set; }

        private readonly DefaultNameSpaceNode defaultNameSpace;
        private readonly CreatedNameSpaceCollectionNode createdNameSpaces;

        public SemanticAnalyser(SyntaxTreeCollection _collection)
        {
            Collection = _collection;

            defaultNameSpace = new DefaultNameSpaceNode();
            createdNameSpaces = new CreatedNameSpaceCollectionNode();
        }

        public SugarPackage Analyse()
        {
            foreach (var tree in Collection)
                StructureTree(tree.BaseNode);

            createdNameSpaces.Print("", true);
            defaultNameSpace.Print("", true);

            package = new SugarPackage(defaultNameSpace, createdNameSpaces);

            var import = new ImportStatementService(defaultNameSpace, createdNameSpaces).Validate();

            Console.WriteLine(import);

            //CreateCollectionGlobalMembers(defaultNameSpace);
            //CreateCollectionGlobalMembers(createdNameSpaces);

            return package;
        }

        private void StructureTree(Node node)
        {
            bool allowImports = true;
            var importStatements = new List<ImportNode>();

            foreach (var child in node.GetChildren())
            {
                switch (child.NodeType)
                {
                    case NodeType.Import:
                        if (!allowImports)
                            throw new Exception("Import statements must preceed all definitions");
                        importStatements.Add((ImportNode)child);
                        break;
                    case NodeType.Namespace:
                        allowImports = false;
                        AnalyseNameSpace((NamespaceNode)child, importStatements);
                        break;
                    case NodeType.Enum:
                    case NodeType.Class:
                    case NodeType.Struct:
                    case NodeType.Interface:
                        allowImports = false;
                        defaultNameSpace.AddEntity(CreateDataType((UDDataTypeNode)child, defaultNameSpace, importStatements));
                        break;
                    default:
                        throw new Exception("Invalid statement");
                }
            }
        }

        private void AnalyseNameSpace(NamespaceNode node, List<ImportNode> importStatements)
        {
            var name = node.Name;
            INameSpaceCollection collection = createdNameSpaces;

            while (name != null)
            {
                switch(name.NodeType)
                {
                    case NodeType.Dot:
                        var dotExpression = (DotExpression)name;
                        var lhs = (IdentifierNode)dotExpression.LHS;

                        collection = EvaluateResult(lhs);
                        name = dotExpression.RHS;
                        break;
                    case NodeType.Variable:
                        var converted = (IdentifierNode)name;
                        collection = EvaluateResult(converted);

                        name = null;
                        break;
                    default:
                        throw new Exception("Parser ain't working mate");
                }
            }

            var dataTypeCollection = (IDataTypeCollection)collection;

            foreach(var child in node.GetDataTypes())
            {
                switch(child.NodeType)
                {
                    case NodeType.Empty:
                        break;
                    case NodeType.Enum:
                    case NodeType.Class:
                    case NodeType.Struct:
                    case NodeType.Interface:
                        dataTypeCollection.AddEntity(CreateDataType((UDDataTypeNode)child, dataTypeCollection, importStatements));
                        break;
                    default:
                        throw new Exception("invalid statement");
                }
            }

            INameSpaceCollection EvaluateResult(IdentifierNode identifier)
            {
                var result = collection.TryFindNameSpace(identifier);
                if (result == null)
                {
                    var created = new CreatedNameSpaceNode(identifier);
                    collection.AddEntity(created);

                    return created;
                }
                else
                {
                    return result;
                }
            }
        }

        private DataType CreateDataType(UDDataTypeNode dataTypeNode, IDataTypeCollection collection, List<ImportNode> importStatements)
        {
            DataType createdType;

            var identifier = (IdentifierNode)dataTypeNode.Name;
            if (collection.TryFindDataType(identifier) != null)
                throw new Exception($"Type {identifier.Value} already exists");

            switch(dataTypeNode.NodeType)
            {
                case NodeType.Enum:
                    createdType =  new EnumType(identifier, importStatements, (EnumNode)dataTypeNode);
                    break;
                case NodeType.Class:
                    createdType =  new ClassType(identifier, importStatements, (ClassNode)dataTypeNode);
                    break;
                case NodeType.Struct:
                    createdType =  new StructType(identifier, importStatements, (StructNode)dataTypeNode);
                    break;
                default:
                    createdType =  new InterfaceType(identifier, importStatements, (InterfaceNode)dataTypeNode);
                    break;
            }

            foreach(var child in dataTypeNode.GetChildren())
            {
                switch(child.NodeType)
                {
                    case NodeType.Enum:
                    case NodeType.Class:
                    case NodeType.Struct:
                    case NodeType.Interface:
                        createdType.AddEntity(CreateDataType((UDDataTypeNode)child, createdType, importStatements));
                        break;
                }
            }

            return createdType;
        }

        private void ValidateImportStatements(INameSpaceCollection collection)
        {
            
        }

        

        

        

        private CreatedNameSpaceNode ValidateNameSpaceNode(Node name)
        {
            var result = FindPossibleNamespaceNode(name);

            if(!result.CompleteValidation)
                throw new Exception($"namespace not found");

            return result.CreatedNameSpace;
        }

        private FindNamespaceResult FindPossibleNamespaceNode(Node name)
        {
            Node current = name;
            INameSpaceCollection collection = createdNameSpaces;

            while(true)
            {
                switch (current.NodeType)
                {
                    case NodeType.Dot:
                        var dot = (DotExpression)current;
                        var result = collection.TryFindNameSpace((IdentifierNode)dot.LHS);
                        if (result == null)
                            return new FindNamespaceResult(false, (CreatedNameSpaceNode)collection);

                        current = dot.RHS;
                        collection = result;
                        break;
                    case NodeType.Variable:
                        result = collection.TryFindNameSpace((IdentifierNode)current);

                        if(result == null)
                            return new FindNamespaceResult(false, (CreatedNameSpaceNode)collection);
                        else
                            return new FindNamespaceResult(true, result);
                    default:
                        throw new Exception("Parser aint working mate");
                }
            }
        }

        private DataType ValidateDataTypeExistance(Node name)
        {
            var findResult = FindPossibleNamespaceNode(name);

            if (findResult.CompleteValidation)
                throw new Exception("type expected, namespace found");

            var nameSpace = findResult.CreatedNameSpace;

            var current = name;
            while (true)
                if (current.NodeType == NodeType.Dot)
                {
                    var dot = (DotExpression)current;

                    current = dot.RHS;
                    if (((IdentifierNode)dot.LHS).Value == nameSpace.Name)
                        break;
                }
                else
                    break;

            var dataTypeCollection = (IDataTypeCollection)nameSpace;

            while(true)
            {
                switch (current.NodeType)
                {
                    case NodeType.Dot:
                        var dot = (DotExpression)current;
                        var result = Validate((IdentifierNode)dot.LHS);

                        current = dot.RHS;
                        dataTypeCollection = result;
                        break;
                    case NodeType.Variable:
                        return Validate((IdentifierNode)current);
                    default:
                        throw new Exception("Parser aint working mate");
                }
            }

            DataType Validate(IdentifierNode name)
            {
                var result = dataTypeCollection.TryFindDataType(name);

                if (result == null)
                    throw new Exception("data Type doesn't exist");

                return result;
            }
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
            switch (dataType.TypeEnum)
            {
                case DataTypeEnum.Class:
                    CreateClassMembers((ClassType)dataType);
                    break;
            }
        }

        private void CreateClassMembers(ClassType classType)
        {
            foreach(var node in classType.Skeleton.GetChildren())
            {
                switch (node.NodeType)
                {
                    case NodeType.Initialise:
                    case NodeType.Declaration:
                        var creationNode = (VariableCreationNode)node;
                        var type = (CustomTypeNode)creationNode.Type;

                        var name = (IdentifierNode)creationNode.Name;
                        var describer = AnalyseDescriber((DescriberNode)creationNode.Describer);
                        
                        if (creationNode.CreationType == NodeType.Variable)
                        {
                            VariableCreationStmt variableCreationNode;
                            if (creationNode.NodeType == NodeType.Declaration)
                                variableCreationNode = new VariableDeclarationStmt(classType.FindReferencedType(type.CustomType, defaultNameSpace, createdNameSpaces), name, describer, false);
                            else
                                variableCreationNode = new VariableInitialisationStmt(classType.FindReferencedType(type.CustomType, defaultNameSpace, createdNameSpaces), name, describer, false, (ExpressionNode)((InitializeNode)node).Value);

                            if (!variableCreationNode.ValidateDescriber())
                                throw new Exception("The describer is not valid on this item");

                            classType.AddDeclaration(variableCreationNode);
                            break;
                        }

                        PropertyCreationStmt propertyCreationNode;
                        var propertyType = creationNode.Name.NodeType;

                        switch(propertyType)
                        {
                            case NodeType.PropertyGet:
                                var getNode = (PropertyGetNode)creationNode.Name;
                                propertyCreationNode = CreateStatement(creationNode.NodeType, getNode.Get, null);
                                break;
                            case NodeType.PropertySet:
                                var setNode = (PropertySetNode)creationNode.Name;
                                propertyCreationNode = CreateStatement(creationNode.NodeType, null, setNode.Set);
                                break;
                            default:
                                var getSetNode = (PropertyGetSetNode)creationNode.Name;
                                propertyCreationNode = CreateStatement(creationNode.NodeType, getSetNode.Get, getSetNode.Set);
                                break;
                        }

                        if (!propertyCreationNode.ValidateDescriber())
                            throw new Exception("The describer is not valid on this item");

                        classType.AddDeclaration(propertyCreationNode);

                        PropertyCreationStmt CreateStatement(NodeType nodeType, Node get, Node set)
                        {
                            if (nodeType == NodeType.Declaration)
                                return new PropertyDeclarationStmt(classType.FindReferencedType(type.CustomType, defaultNameSpace, createdNameSpaces), name, describer, get, set);
                            else
                                return new PropertyInitialisationStmt(classType.FindReferencedType(type.CustomType, defaultNameSpace, createdNameSpaces), name, describer, get, set, (ExpressionNode)((InitializeNode)node).Value);
                        }
                        break;
                    case NodeType.FunctionDeclaration:
                        var functionDeclaration = (FunctionDeclarationNode)node;
                        var returnType = classType.FindReferencedType(functionDeclaration.Type, defaultNameSpace, createdNameSpaces);
                        var arguments = CreateArguments((FunctionDeclarationArgumentsNode)functionDeclaration.Arguments, classType);

                        var method = new MethodDeclarationStmt(returnType, AnalyseDescriber((DescriberNode)functionDeclaration.Describer), arguments, functionDeclaration);

                        if (!method.ValidateDescriber())
                            throw new Exception("The describer is not valid on this item");

                        classType.AddDeclaration(method);
                        break;
                }
            }
        }

        private Describer AnalyseDescriber(DescriberNode describerNode) => new Describer(GatherDescribers(describerNode));

        private DescriberEnum GatherDescribers(DescriberNode describerNode)
        {
            DescriberEnum describer = 0;

            for(int i = 0; i < describerNode.ChildCount; i++)
            {
                var child = (DescriberKeywordNode)describerNode[i];

                switch(child.Keyword.SyntaxKind)
                {
                    case SyntaxKind.Public:
                        describer |= DescriberEnum.Public;
                        break;
                    case SyntaxKind.Private:
                        describer |= DescriberEnum.Private;
                        break;
                    case SyntaxKind.Protected:
                        describer |= DescriberEnum.Protected;
                        break;

                    case SyntaxKind.Static:
                        describer |= DescriberEnum.Static;
                        break;

                    case SyntaxKind.Sealed:
                        describer |= DescriberEnum.Sealed;
                        break;
                }
            }

            return describer;
        }

        private FunctionArguments CreateArguments(FunctionDeclarationArgumentsNode argumentsNode, DataType type)
        {
            var arguments = new FunctionArguments();


            for(int i = 0; i < argumentsNode.ChildCount; i++)
            {
                var child = (FunctionDeclarationArgumentNode)argumentsNode[i];

                arguments.Add(((IdentifierNode)child.Name).Value, type.FindReferencedType(child.Type, defaultNameSpace, createdNameSpaces));
            }

            return arguments;
        }
    }
}
