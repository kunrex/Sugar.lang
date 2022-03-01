using System;
using System.Collections.Generic;

using Sugar.Language.Parsing;
using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.UDDataTypes;
using Sugar.Language.Parsing.Nodes.Expressions.Associative;

using Sugar.Language.Semantics.ActionTrees;
using Sugar.Language.Semantics.ActionTrees.Namespaces;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Interfaces.Namespaces;
using Sugar.Language.Parsing.Nodes.Statements;
using Sugar.Language.Parsing.Nodes.UDDataTypes.Enums;
using Sugar.Language.Semantics.Analysis.Structure;
using Sugar.Language.Semantics.ActionTrees.Enums;

namespace Sugar.Language.Semantics.Analysis
{
    internal sealed class SemanticAnalyser
    {
        public SyntaxTree Base { get; private set; }

        private readonly DefaultNameSpaceNode defaultNameSpace;
        private readonly CreatedNameSpaceCollectionNode createdNameSpaces;

        public SemanticAnalyser(SyntaxTree _base)
        {
            Base = _base;

            defaultNameSpace = new DefaultNameSpaceNode();
            createdNameSpaces = new CreatedNameSpaceCollectionNode();
        }

        public SugarPackage Analyse()
        {
            StructureNamespacesAndTypes(Base.BaseNode);

            createdNameSpaces.Print("");
            defaultNameSpace.Print("", true);

            ValidateImportStatements(defaultNameSpace);
            for (int i = 0; i < createdNameSpaces.NamespaceCount; i++)
                ValidateImportStatements(createdNameSpaces[i]);

            return new SugarPackage(defaultNameSpace, createdNameSpaces);
        }

        private void StructureNamespacesAndTypes(Node node)
        {
            bool allowImports = true;
            List<ImportNode> importStatements = new List<ImportNode>();

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
                        defaultNameSpace.AddDataType(CreateDataType((UDDataTypeNode)child, defaultNameSpace, importStatements));
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

                        collection = EvaluateResult(lhs, collection);
                        name = dotExpression.RHS;
                        break;
                    case NodeType.Variable:
                        var converted = (IdentifierNode)name;
                        collection = EvaluateResult(converted, collection);

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
                    case NodeType.Enum:
                    case NodeType.Class:
                    case NodeType.Struct:
                    case NodeType.Interface:
                        dataTypeCollection.AddDataType(CreateDataType((UDDataTypeNode)child, dataTypeCollection, importStatements));
                        break;
                    default:
                        throw new Exception("invalid statement");
                }
            }

            INameSpaceCollection EvaluateResult(IdentifierNode identifier, INameSpaceCollection collection)
            {
                var result = collection.TryFindNameSpace(identifier);
                if (result == null)
                {
                    var created = new CreatedNameSpaceNode(identifier);
                    collection.AddNameSpace(created);

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
                        createdType.AddDataType(CreateDataType((UDDataTypeNode)child, createdType, importStatements));
                        break;
                }
            }

            return createdType;
        }

        private void ValidateImportStatements(CreatedNameSpaceNode namespaceNode)
        {
            for (int i = 0; i < namespaceNode.NamespaceCount; i++)
                ValidateImportStatements(namespaceNode[i]);

            ValidateImportStatements((IDataTypeCollection)namespaceNode);
        }

        private void ValidateImportStatements(IDataTypeCollection collection)
        {
            for(int i = 0; i < collection.DatatypeCount; i++)
            {
                var dataType = collection[i];

                foreach (var import in dataType.GetReferencedNamespaces())
                {
                    switch(import.EntityType)
                    {
                        case UDDataType.Namespace:
                            ValidateNameSpaceNode(import.Name);
                            break;
                        case UDDataType.Enum:
                            var enumType = ValidateDataExistance(import.Name);

                            if (enumType.TypeEnum != DataTypeEnum.Enum)
                                throw new Exception($"{enumType.Name} is not an enum");
                            break;
                        case UDDataType.Class:
                            var classType = ValidateDataExistance(import.Name);

                            if (classType.TypeEnum != DataTypeEnum.Class)
                                throw new Exception($"{classType.Name} is not a class");
                            break;
                        case UDDataType.Struct:
                            var structType = ValidateDataExistance(import.Name);

                            if (structType.TypeEnum != DataTypeEnum.Struct)
                                throw new Exception($"{structType.Name} is not a struct");
                            break;
                        default:
                            var interfaceType = ValidateDataExistance(import.Name);

                            if (interfaceType.TypeEnum != DataTypeEnum.Interface)
                                throw new Exception($"{interfaceType.Name} is not an interface");
                            break;
                    }
                }
            }
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

        private DataType ValidateDataExistance(Node name)
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
    }
}
