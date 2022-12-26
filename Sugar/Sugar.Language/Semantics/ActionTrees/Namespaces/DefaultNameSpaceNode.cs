using System;
using System.Linq;
using System.Collections.Generic;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.Analysis.BuiltInTypes.Enums;
using Sugar.Language.Semantics.ActionTrees.Interfaces.Collections;
using Sugar.Language.Parsing.Nodes.Values;

namespace Sugar.Language.Semantics.ActionTrees.Namespaces
{
    internal sealed class DefaultNameSpaceNode : BaseNameSpaceNode
    {
        private readonly Dictionary<string, DataType> internalDataTypes;
        public override ActionNodeEnum ActionNodeType { get => ActionNodeEnum.DefaultNameSpace; }

        public IList<DataType> InternalDataTypes { get => internalDataTypes.Values.ToList(); }

        public DefaultNameSpaceNode() : base()
        {
            internalDataTypes = new Dictionary<string, DataType>();
        }

        public IDataTypeCollection AddInternalDataType(DataType dataType)
        {
            internalDataTypes.Add(dataType.Name, dataType);
            dataType.SetParent(this);

            return this;
        }

        public override DataType TryFindDataType(IdentifierNode identifier)
        {
            var value = identifier.Value;
            if (internalDataTypes.ContainsKey(value))
                return internalDataTypes[value];

            return base.TryFindDataType(identifier);
        }

        public DataType GetInternalDataType(TypeEnum dataTypeEnum) => internalDataTypes[dataTypeEnum.ToString()];

        protected override void PrintChildren(string indent)
        {
            for (int i = 0; i < dataTypes.Count; i++)
                dataTypes[i].Print(indent, i == dataTypes.Count - 1);
        }

        public override string ToString() => $"Default Name Space";
    }
}
