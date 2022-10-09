using System;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.PropertyCreation;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global
{
    internal sealed class IndexerCreationStmt : GlobalFunctionCreationStmt<IIndexerContainer>
    {
        private new PropertyCreationStmt nodeBody;
        public new PropertyCreationStmt NodeBody { get => nodeBody; }

        public IndexerCreationStmt(DataType _creationType, Describer _describer, FunctionArguments _arguments, PropertyCreationStmt _nodeBody) : base(
            _creationType,
            _creationType.Name,
            _describer,
            _arguments,
            null)
        {
            nodeBody = _nodeBody;
        }

        public override string ToString() => $"Indexer Declaration Node";
    }
}
