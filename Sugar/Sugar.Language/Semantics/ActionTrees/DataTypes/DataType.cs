using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.Interfaces.Namespaces;

namespace Sugar.Language.Semantics.ActionTrees.DataTypes
{
    internal abstract class DataType : ActionTreeNode, IDataTypeCollection
    {
        public abstract DataTypeEnum TypeEnum { get; }
        public IdentifierNode Name { get; private set; }

        protected readonly List<DataType> subTypes;

        public DataType(IdentifierNode _name)
        {
            Name = _name;

            subTypes = new List<DataType>();
        }

        public DataType TryFindDataType(IdentifierNode identifier)
        {
            foreach (var type in subTypes)
                if (type.Name.Value == identifier.Value)
                    return type;

            return null;
        }

        public IDataTypeCollection AddDataType(DataType datatypeToAdd)
        {
            subTypes.Add(datatypeToAdd);

            return this;
        }

        protected override void PrintChildren(string indent)
        {
            for (int i = 0; i < subTypes.Count; i++)
                subTypes[i].Print(indent, i == subTypes.Count - 1);
        }
    }
}
