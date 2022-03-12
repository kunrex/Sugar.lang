using System;
using System.Collections.Generic;
using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Interfaces.Namespaces;

namespace Sugar.Language.Semantics.ActionTrees.Namespaces
{
    internal abstract class BaseNameSpaceNode : ParentableActionTreeNode<INameSpaceCollection>, IDataTypeCollection
    {
        protected readonly List<DataType> dataTypes;

        public int DataTypeCount { get => dataTypes.Count; }

        public DataType this[int index]
        {
            get => dataTypes[index];
        }

        public BaseNameSpaceNode()  
        {
            dataTypes = new List<DataType>();
        }

        public IDataTypeCollection AddDataType(DataType dataType)
        {
            dataTypes.Add(dataType);

            return this;
        }

        public DataType TryFindDataType(IdentifierNode identifier)
        {
            foreach (var type in dataTypes)
                if (type.Name.Value == identifier.Value)
                    return type;

            return null;
        }
    }
}
