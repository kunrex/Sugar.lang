using System;

using Sugar.Language.Parsing.Nodes.Types.Enums;

namespace Sugar.Language.Parsing.Nodes.Types.Subtypes
{
    internal sealed class ConstructorTypeNode : TypeNode
    {
        public override TypeNodeEnum Type { get => TypeNodeEnum.Constructor; }

        public ConstructorTypeNode(TypeNode _type) : base(_type)
        {

        }

        public override string ToString() => $"Constructor Type Node";
    }
}
