using System;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.DataTypes;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces.Namespaces
{
    internal interface IDataTypeCollection
    {
        public DataType TryFindDataType(IdentifierNode identifier);
        public IDataTypeCollection AddDataType(DataType datatypeToAdd);
    }
}
