using System;

using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes
{
    internal interface IIndexerContainer : IContainer<IndexerCreationStmt, IIndexerContainer>
    {
        public IndexerCreationStmt TryFindIndexerCreationStatement(DataType external, IFunctionArguments arguments);
    }
}
