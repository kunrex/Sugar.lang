using System;

using Sugar.Language.Parsing.Nodes.Types.Enums;

namespace Sugar.Language.Parsing.Nodes.Types.Subtypes
{
    internal sealed class AnonymousTypeNode : TypeNode
    {
        public override TypeNodeEnum Type { get => TypeNodeEnum.Anonymous; }

        public AnonymousTypeNode()
        {

        }

        public override string ToString() => $"Anonymous Type Node";
    }
}
