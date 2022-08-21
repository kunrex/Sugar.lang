using System;

using Sugar.Language.Parsing.Nodes;

using Sugar.Language.Semantics.ActionTrees.DataTypes;

namespace Sugar.Language.Semantics.Services.Interfaces
{
    internal interface ISubTypeSearcherService : ISemanticService
    {
        public DataType TryFindReferencedType(Node type);
    }
}
