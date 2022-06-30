using System;
using System.Collections.Generic;

using Sugar.Language.Exceptions.Analytics.NameSpaceStructurisation;
using Sugar.Language.Parsing;
using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Expressions.Associative;
using Sugar.Language.Parsing.Nodes.Statements;
using Sugar.Language.Parsing.Nodes.UDDataTypes;
using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Interfaces.Collections;
using Sugar.Language.Semantics.ActionTrees.Namespaces;
using Sugar.Language.Semantics.Analysis;
using Sugar.Language.Semantics.Services.Interfaces;

namespace Sugar.Language.Semantics.Services.Implementations
{
    internal class NameSpaceStructureService : INameSpaceStructureService
    {
        private readonly DefaultNameSpaceNode defaultNameSpace;
        private readonly CreatedNameSpaceCollectionNode createdNameSpaces;

        private readonly SyntaxTreeCollection collection;
        private readonly SemanticsResult semanticsResult;

        public NameSpaceStructureService(SyntaxTreeCollection _collection)
        {
            collection = _collection;

            defaultNameSpace = new DefaultNameSpaceNode();
            createdNameSpaces = new CreatedNameSpaceCollectionNode();

            semanticsResult = new SemanticsResult("NameSpace Structurisation");
        }

        public SemanticsResult Validate()
        {
            foreach (var tree in collection)
                StructureTree(tree.BaseNode);

            return semanticsResult.Build(new object[] { defaultNameSpace, createdNameSpaces });
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
                            semanticsResult.Add(new ImportStatementPrecedenceException((ImportNode)child));
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
                        semanticsResult.Add(new InvalidStatementException("file", $"namespace{(allowImports ? ", import statement" : "")} or data type declaration"));
                        break;
                }
            }
        }

        private void AnalyseNameSpace(NamespaceNode node, List<ImportNode> importStatements)
        {
            var name = node.Name;
            INameSpaceCollection collection = createdNameSpaces;

            while (name != null)
            {
                switch (name.NodeType)
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
                }
            }

            var dataTypeCollection = (IDataTypeCollection)collection;

            foreach (var child in node.GetDataTypes())
            {
                switch (child.NodeType)
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
                        semanticsResult.Add(new InvalidStatementException("namespace declaration", $"data type declaration"));
                        break;
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

            switch (dataTypeNode.NodeType)
            {
                case NodeType.Enum:
                    createdType = new EnumType(identifier, importStatements, (EnumNode)dataTypeNode);
                    break;
                case NodeType.Class:
                    createdType = new ClassType(identifier, importStatements, (ClassNode)dataTypeNode);
                    break;
                case NodeType.Struct:
                    createdType = new StructType(identifier, importStatements, (StructNode)dataTypeNode);
                    break;
                default:
                    createdType = new InterfaceType(identifier, importStatements, (InterfaceNode)dataTypeNode);
                    break;
            }

            foreach (var child in dataTypeNode.GetChildren())
            {
                switch (child.NodeType)
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
    }
}
