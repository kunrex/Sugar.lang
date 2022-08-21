using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Statements;
using Sugar.Language.Parsing.Nodes.Expressions.Associative;
using Sugar.Language.Parsing.Nodes.UDDataTypes.Enums;

using Sugar.Language.Semantics.Analysis;
using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.Services.Interfaces;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Namespaces;
using Sugar.Language.Semantics.ActionTrees.Interfaces.Collections;
using Sugar.Language.Semantics.Services.Implementations.Structures;

using Sugar.Language.Exceptions.Analytics.ImportStatements;

namespace Sugar.Language.Semantics.Services.Implementations
{
    internal class ImportStatementService : IImportStatementService
    {
        private readonly DefaultNameSpaceNode defaultNameSpace;
        private readonly CreatedNameSpaceCollectionNode createdNameSpaces;

        private readonly SemanticsResult semanticsResult;

        public ImportStatementService(DefaultNameSpaceNode _defaultNameSpace, CreatedNameSpaceCollectionNode _createdNameSpaces)
        {
            defaultNameSpace = _defaultNameSpace;
            createdNameSpaces = _createdNameSpaces;

            semanticsResult = new SemanticsResult("Import Statement Validation");
        }

        public SemanticsResult Validate()
        {
            for (int i = 0; i < createdNameSpaces.NameSpaceCount; i++)
                ValidateNameSpace(createdNameSpaces.GetSubNameSpace(i));

            ValidateDataTypeCollection(defaultNameSpace);

            return semanticsResult.Build();
        }

        private void ValidateNameSpace(CreatedNameSpaceNode namespaceNode)
        {
            ValidateDataTypeCollection(namespaceNode);

            for (int i = 0; i < namespaceNode.NameSpaceCount; i++)
                ValidateNameSpace(namespaceNode.GetSubNameSpace(i));
        }

        private void ValidateDataTypeCollection(IDataTypeCollection dataTypeCollection)
        {
            for (int i = 0; i < dataTypeCollection.DataTypeCount; i++)
            {
                var dataType = dataTypeCollection.GetSubDataType(i);
                ValidateDataType(dataType);

                ValidateDataTypeCollection(dataType);
            }
        }

        private void ValidateDataType(DataType dataType)
        {
            ValidateImportStatements(dataType.ReferencedImports, dataType);

            dataType.ReferenceParentNameSpaces();
        }

        private void ValidateImportStatements(IEnumerable<ImportNode> referencedNameSpaces, DataType dataType)
        {
            var categories = new Dictionary<IdentifierNode, List<NodeGroup>>();

            foreach(var importNode in referencedNameSpaces)
            {
                var name = importNode.Name;

                switch(name.NodeType)
                {
                    case NodeType.Dot:
                        CreateCategories(categories, importNode, (DotExpression)name);
                        break;
                    case NodeType.Variable:
                        TryReferenceNameSpace((IdentifierNode)name, null, createdNameSpaces, dataType, importNode, false);
                        break;
                }
            }

            foreach (var pair in categories)
                TryReferenceNameSpace(pair.Key, pair.Value, createdNameSpaces, dataType, false);
        }

        private void ValidateImportStatements(IEnumerable<NodeGroup> nodeGroups, DataType dataType, INameSpaceCollection collection)
        {
            var categories = new Dictionary<IdentifierNode, List<NodeGroup>>();

            foreach (var nodeGroup in nodeGroups)
            {
                switch (nodeGroup.Value.NodeType)
                {
                    case NodeType.Dot:
                        CreateCategories(categories, nodeGroup.ImportNode, (DotExpression)nodeGroup.Value);
                        break;
                    case NodeType.Variable:
                        TryReferenceNameSpace((IdentifierNode)nodeGroup.Value, null, collection, dataType, nodeGroup.ImportNode, true);
                        break;
                }
            }

            foreach (var pair in categories)
                TryReferenceNameSpace(pair.Key, pair.Value, collection, dataType, true);
        }

        private void ValidateImportStatements(IEnumerable<NodeGroup> nodeGroups, DataType dataType, IDataTypeCollection collection)
        {
            var categories = new Dictionary<IdentifierNode, List<NodeGroup>>();

            foreach (var nodeGroup in nodeGroups)
            {
                switch (nodeGroup.Value.NodeType)
                {
                    case NodeType.Dot:
                        CreateCategories(categories, nodeGroup.ImportNode, (DotExpression)nodeGroup.Value);
                        break;
                    case NodeType.Variable:
                        TryReferenceDataType((IdentifierNode)nodeGroup.Value, null, collection, dataType, nodeGroup.ImportNode);
                        break;
                }
            }

            foreach (var pair in categories)
                TryReferenceDataType(pair.Key, pair.Value, collection, dataType);
        }

        private void CreateCategories(Dictionary<IdentifierNode, List<NodeGroup>> categories, ImportNode importNode, DotExpression dotExpression)
        {
            var lhs = (IdentifierNode)dotExpression.LHS;

            bool found = false;
            var name = lhs.Value;
            foreach(var pair in categories)
                if (pair.Key.Value == name)
                {
                    found = true;
                    pair.Value.Add(new NodeGroup(dotExpression.RHS, importNode));
                }

            if (!found)
                categories.Add(lhs, new List<NodeGroup>() { new NodeGroup(dotExpression.RHS, importNode) });
        }

        private void TryReferenceDataType(IdentifierNode identifier, List<NodeGroup> nodeGroups, IDataTypeCollection collection, DataType dataType)
        {
            var result = collection.TryFindDataType(identifier);

            if (result == null)
                semanticsResult.Add(new NameSpaceNotFoundException(nodeGroups[0].ImportNode));

            ValidateImportStatements(nodeGroups, dataType, result);
        }

        private void TryReferenceDataType(IdentifierNode identifier, List<NodeGroup> nodeGroups, IDataTypeCollection collection, DataType dataType, ImportNode importNode)
        {
            var result = collection.TryFindDataType(identifier);

            if (result == null)
                semanticsResult.Add(new NameSpaceNotFoundException(importNode));

            MatchExpectedImportType(nodeGroups, result, dataType, importNode);
        }

        private void TryReferenceNameSpace(IdentifierNode identifier, List<NodeGroup> nodeGroups, INameSpaceCollection collection, DataType dataType, bool isCreatedNameSpace)
        {
            var result = ValidateNameSpaceResult(nodeGroups, identifier, collection, dataType, isCreatedNameSpace);
            if (result == null)
                return;

            ValidateImportStatements(nodeGroups, dataType, (INameSpaceCollection)result);
        }

        private void TryReferenceNameSpace(IdentifierNode identifier, List<NodeGroup> nodeGroups, INameSpaceCollection collection, DataType dataType, ImportNode importNode, bool isCreatedNameSpace)
        {
            var result = ValidateNameSpaceResult(nodeGroups, identifier, collection, dataType, importNode, isCreatedNameSpace);
            if (result == null)
                return;

            if (nodeGroups == null)
            {
                if (importNode.EntityType != UDDataType.Namespace)
                    semanticsResult.Add(new InvalidEntityTypeException(importNode, DataTypeEnum.Namespace, (DataTypeEnum)importNode.EntityType));

                dataType.ReferenceNameSpace(result);
            }
            else
                ValidateImportStatements(nodeGroups, dataType, (INameSpaceCollection)result);
        }

        private CreatedNameSpaceNode ValidateNameSpaceResult(List<NodeGroup> nodeGroups, IdentifierNode identifier, INameSpaceCollection collection, DataType dataType, bool isCreatedNameSpace)
        {
            var result = collection.TryFindNameSpace(identifier);

            if (result == null)
            {
                if (isCreatedNameSpace)
                {
                    var nameSpace = (CreatedNameSpaceNode)collection;
                    var dataTypeResult = nameSpace.TryFindDataType(identifier);

                    if (dataTypeResult != null)
                    {
                        ValidateImportStatements(nodeGroups, dataType, dataTypeResult);
                        return null;
                    }
                }

                semanticsResult.Add(new NameSpaceNotFoundException(nodeGroups[0].ImportNode));
            }

            return result;
        }

        private CreatedNameSpaceNode ValidateNameSpaceResult(List<NodeGroup> nodeGroups, IdentifierNode identifier, INameSpaceCollection collection, DataType dataType, ImportNode importNode, bool isCreatedNameSpace)
        {
            var result = collection.TryFindNameSpace(identifier);

            if (result == null)
            {
                if (isCreatedNameSpace)
                {
                    var nameSpace = (CreatedNameSpaceNode)collection;
                    var dataTypeResult = nameSpace.TryFindDataType(identifier);

                    if (dataTypeResult != null)
                    {
                        MatchExpectedImportType(nodeGroups, dataTypeResult, dataType, importNode);
                        return null;
                    }
                }

                semanticsResult.Add(new NameSpaceNotFoundException(importNode));
            }

            return result;
        }

        private void MatchExpectedImportType(List<NodeGroup> nodeGroups, DataType result, DataType dataType, ImportNode importNode)
        {
            if (nodeGroups == null)
            {
                if ((UDDataType)result.TypeEnum != importNode.EntityType)
                    semanticsResult.Add(new InvalidEntityTypeException(importNode, result.TypeEnum, (DataTypeEnum)importNode.EntityType));

                dataType.ReferenceDataType(result);
            }
            else
                ValidateImportStatements(nodeGroups, dataType, result);
        }
    }
}
