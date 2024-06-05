using System;

using Sugar.Language.Parsing.Nodes.Types.Enums;

namespace Sugar.Language.Parsing.Nodes.Types.Subtypes
{
    internal sealed class CustomTypeNode : TypeNode
    {
        public override TypeNodeEnum Type => TypeNodeEnum.Custom;

        public CustomTypeNode(TypeNode _type) : base(_type)
        {
            
        }

        public override string ToString() => $"Custom Type Node";
    }
}
