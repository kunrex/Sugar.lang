using System;
using System.Collections.Generic;

using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.Analysis.BuiltInTypes.Enums;
using Sugar.Language.Semantics.ActionTrees.Interfaces.Collections;

namespace Sugar.Language.Semantics.ActionTrees.Namespaces
{
    internal sealed class DefaultNameSpaceNode : BaseNameSpaceNode
    {
        private readonly Dictionary<string, DataType> internalDataTypes;

        public DefaultNameSpaceNode() : base()
        {
            internalDataTypes = new Dictionary<string, DataType>();
        }

        public IDataTypeCollection AddInternalDataType(DataType dataType)
        {
            internalDataTypes.Add(dataType.Name, dataType);

            return this;
        }

        public DataType GetInternalDataType(TypeEnum dataTypeEnum) => internalDataTypes[dataTypeEnum.ToString()];

        public override string ToString() => $"Default Name Space";

        protected override void PrintChildren(string indent)
        {
            for (int i = 0; i < dataTypes.Count; i++)
                dataTypes[i].Print(indent, i == dataTypes.Count - 1);
        }
    }
}
