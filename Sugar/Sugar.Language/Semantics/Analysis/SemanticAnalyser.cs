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
            var baseNode = Base.BaseNode;

            switch (baseNode.NodeType)
            {
                case NodeType.Group:
                    foreach (var child in baseNode.GetChildren())
                        AnalyseNode(child);
                    break;
                default:
                    AnalyseNode(baseNode);
                    break;
            }

            createdNameSpaces.Print("");
            return new SugarPackage(defaultNameSpace, createdNameSpaces);
        }

        private void AnalyseNode(Node node)
        {
            switch(node.NodeType)
            {
                case NodeType.Group:
                    foreach (var child in node.GetChildren())
                        AnalyseNode(child);
                    break;
                default:

                case NodeType.Namespace:
                    AnalyseNameSpace((NamespaceNode)node);
                    break;
                case NodeType.Enum:
                case NodeType.Class:
                case NodeType.Struct:
                case NodeType.Interface:
                    defaultNameSpace.AddDataType(CreateDataType((UDDataTypeNode)node, defaultNameSpace));
                    break;
            }
        }

        private void AnalyseNameSpace(NamespaceNode node)
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
                        dataTypeCollection.AddDataType(CreateDataType((UDDataTypeNode)child, dataTypeCollection));
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

        private DataType CreateDataType(UDDataTypeNode dataTypeNode, IDataTypeCollection collection)
        {
            DataType createdType;

            var identifier = (IdentifierNode)dataTypeNode.Name;
            if (collection.TryFindDataType(identifier) != null)
                throw new Exception($"Type {identifier.Value} already exists");

            switch(dataTypeNode.NodeType)
            {
                case NodeType.Enum:
                    createdType =  new EnumType(identifier);
                    break;
                case NodeType.Class:
                    createdType =  new ClassType(identifier);
                    break;
                case NodeType.Struct:
                    createdType =  new StructType(identifier);
                    break;
                default:
                    createdType =  new InterfaceType(identifier);
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
                        createdType.AddDataType(CreateDataType((UDDataTypeNode)child, createdType));
                        break;
                }
            }

            return createdType;
        }
    }
}
