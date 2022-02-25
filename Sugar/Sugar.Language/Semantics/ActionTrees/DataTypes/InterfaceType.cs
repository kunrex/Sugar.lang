using System;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.Enums;

namespace Sugar.Language.Semantics.ActionTrees.DataTypes
{
    internal sealed class InterfaceType : DataType
    {
        public override DataTypeEnum TypeEnum { get => DataTypeEnum.Enum; }

        public InterfaceType(IdentifierNode _name) : base(_name)
        {

        }

        public override string ToString() => $"Enum Node [{Name.Value}]";
    }
}
