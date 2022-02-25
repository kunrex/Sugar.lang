using System;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.Enums;

namespace Sugar.Language.Semantics.ActionTrees.DataTypes
{
    internal sealed class StructType : DataType
    {
        public override DataTypeEnum TypeEnum { get => DataTypeEnum.Enum; }

        public StructType(IdentifierNode _name) : base(_name)
        {

        }

        public override string ToString() => $"Enum Node [{Name.Value}]";
    }
}
