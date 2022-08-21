using System;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes
{
    internal interface IIndexerContainer : IContainer<IndexerCreationStmt, IIndexerContainer>
    {
        public IndexerCreationStmt TryFindIndexerCreationStatement(IdentifierNode identifier);

        public bool IsDuplicateIndexer(DataType indexer);
    }
}
