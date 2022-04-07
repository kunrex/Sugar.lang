using System;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.DataTypes;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces.Namespaces
{
    internal interface IDataTypeCollection : IEntityCollection<DataType, IDataTypeCollection>
    {
        public int DataTypeCount { get; }

        public DataType GetSubDataType(int index);
        public DataType TryFindDataType(IdentifierNode identifier);
    }
}
