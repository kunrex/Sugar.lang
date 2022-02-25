using System;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.Enums;

namespace Sugar.Language.Semantics.ActionTrees.DataTypes
{
    internal sealed class ClassType : DataType
    {
        public override DataTypeEnum TypeEnum { get => DataTypeEnum.Class; }

        public ClassType(IdentifierNode _name) : base(_name)
        {
            
        }

        public override string ToString() => $"Class Node [{Name.Value}]";
    }
}
