using System;
using System.Collections.Generic;

using Sugar.Language.Exceptions.Analysis.Import;
using Sugar.Language.Exceptions.Analysis.Project;

using Sugar.Language.Parsing;
using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.DataTypes;
using Sugar.Language.Parsing.Nodes.Statements;
using Sugar.Language.Parsing.Nodes.Expressions.Associative;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Collections;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes;
using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;
using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.Namespaces;
using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes.Structure;

using Enum = Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes.Enum;

namespace Sugar.Language.Analysis.ProjectCreation
{
    internal sealed class NamespaceConstructor : SemanticService
    {
        private readonly SyntaxTreeCollection wrapper;
        private readonly SyntaxTreeCollection project;

        public NamespaceConstructor(SyntaxTreeCollection _wrapper, SyntaxTreeCollection _project, ProjectTree _projectTree) : base(_projectTree)
        {
            wrapper = _wrapper;
            project = _project;
        }

        public override void Process()
        {
            /*foreach (var tree in wrapper)
            {
                if (tree.BaseNode.NodeType == ParseNodeType.Empty)
                    continue;

                StructureTree((ParseNodeCollection)tree.BaseNode, projectTree.DefaultNamespace.AddInternalDataType);
            }*/

            foreach (var tree in project)
            {
                if (tree.BaseNode.NodeType == ParseNodeType.Empty)
                    continue;

                StructureTree(tree, projectTree.DefaultNamespace.AddEntity);
            }
        }

        private void StructureTree(SyntaxTree tree, Func<DataType, IDataTypeCollection> defaultTypeFunctionCall)
        {
            bool allowImports = true;

            var references = new ReferenceCollection();
            tree.WithReferences(references);

            foreach (var child in (ParseNodeCollection)tree.BaseNode)
            {
                switch (child.NodeType)
                {
                    case ParseNodeType.Import:
                        var import = (ImportNode)child;
                        if (!allowImports)
                            projectTree.WithException(new ImportPrecedenceException(import));

                        references.WithImport(import);
                        break;
                    case ParseNodeType.Namespace:
                        allowImports = false;
                        AnalyseNameSpace((NamespaceNode)child, references);
                        break;
                    case ParseNodeType.Enum:
                    case ParseNodeType.Class:
                    case ParseNodeType.Struct:
                    case ParseNodeType.Interface:
                        allowImports = false;
                        var dataType = (DataTypeNode)child;

                        if (projectTree.DefaultNamespace.TryFindDataType(dataType.Name) == null)
                            defaultTypeFunctionCall.Invoke(CreateDataType(dataType, references));
                        else
                            projectTree.WithException(new DuplicateDataTypeException(dataType.Name.Value));
                        break;
                    default:
                        projectTree.WithException(new InvalidProjectStatementException(child));
                        break;
                }
            }
        }

        private void AnalyseNameSpace(NamespaceNode node, ReferenceCollection references)
        {
            var name = node.Name;
            INamespaceCollection collection = projectTree.ProjectNamespace;

            while (name != null)
            {
                switch (name.NodeType)
                {
                    case ParseNodeType.Dot:
                        var dotExpression = (DotExpression)name;
                        var lhs = (IdentifierNode)dotExpression.LHS;

                        collection = EvaluateResult(lhs);
                        name = dotExpression.RHS;
                        break;
                    case ParseNodeType.Variable:
                        var converted = (IdentifierNode)name;
                        collection = EvaluateResult(converted);

                        name = null;
                        break;
                }
            }

            var dataTypeCollection = (IDataTypeCollection)collection;

            switch(node.Body.NodeType)
            {
                case ParseNodeType.Empty:
                    return;
                case ParseNodeType.Scope:
                    foreach(var child in node.Body as ParseNodeCollection)
                        EvaluateNode(child, dataTypeCollection, references);
                    break;
                default:
                    EvaluateNode(node.Body, dataTypeCollection, references);
                    break;
            }

            INamespaceCollection EvaluateResult(IdentifierNode identifier)
            {
                var result = collection.TryFindNameSpace(identifier);
                if (result == null)
                {
                    var created = new CreatedNamespaceNode(identifier.Value);
                    collection.AddEntity(created);

                    return created;
                }
                else
                    return result;
            }

            void EvaluateNode(ParseNode node, IDataTypeCollection collection, ReferenceCollection references)
            {
                switch (node.NodeType)
                {
                    case ParseNodeType.Empty:
                        break;
                    case ParseNodeType.Enum:
                    case ParseNodeType.Class:
                    case ParseNodeType.Struct:
                    case ParseNodeType.Interface:
                        CreateDataType((DataTypeNode)node, collection, references);
                        break;
                    default:
                        projectTree.WithException(new InvalidProjectStatementException(node));
                        break;
                }
            }
        }

        private void CreateDataType(DataTypeNode dataTypeNode, IDataTypeCollection collection, ReferenceCollection references)
        { 
            if (collection.TryFindDataType(dataTypeNode.Name) == null)
                collection.AddEntity(CreateDataType(dataTypeNode, references));
            else
                projectTree.WithException(new DuplicateDataTypeException(dataTypeNode.Name.Value));
        }

        private DataType CreateDataType(DataTypeNode dataTypeNode, ReferenceCollection references)
        {
            DataType createdType;

            switch (dataTypeNode.NodeType)
            {
                case ParseNodeType.Enum:
                    createdType = new Enum(dataTypeNode.Name.Value, CreateDescriber(dataTypeNode.Describer), (EnumNode)dataTypeNode, references);
                    break;
                case ParseNodeType.Class:
                    createdType = new Class(dataTypeNode.Name.Value, CreateDescriber(dataTypeNode.Describer), (ClassNode)dataTypeNode, references);
                    break;
                case ParseNodeType.Struct:
                    createdType = new Struct(dataTypeNode.Name.Value, CreateDescriber(dataTypeNode.Describer), (StructNode)dataTypeNode, references);
                    break;
                default:
                    createdType = new Interface(dataTypeNode.Name.Value, CreateDescriber(dataTypeNode.Describer), (InterfaceNode)dataTypeNode, references);
                    break;
            }

            switch (dataTypeNode.Body.NodeType)
            {
                case ParseNodeType.Empty:
                    break;
                case ParseNodeType.Scope:
                    foreach (var child in dataTypeNode.Body as ParseNodeCollection)
                        EvaluateNode(child, createdType, references);
                    break;
                default:
                    EvaluateNode(dataTypeNode.Body, createdType, references);
                    break;
            }

            return createdType;

            void EvaluateNode(ParseNode node, IDataTypeCollection collection, ReferenceCollection references)
            {
                switch (node.NodeType)
                {
                    case ParseNodeType.Empty:
                        break;
                    case ParseNodeType.Enum:
                    case ParseNodeType.Class:
                    case ParseNodeType.Struct:
                    case ParseNodeType.Interface:
                        CreateDataType((DataTypeNode)node, collection, references);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}