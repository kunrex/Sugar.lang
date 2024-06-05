using System;

using Sugar.Language.Parsing.Nodes.Types.Enums;

using Sugar.Language.Parsing.Nodes.Values.Generics;

namespace Sugar.Language.Parsing.Nodes.Types.Subtypes
{
    internal sealed class ArrayTypeNode : TypeNode
    {
        public override TypeNodeEnum Type { get => TypeNodeEnum.Array; }

        private readonly GenericCallNode generic;
        public GenericCallNode Generic { get => generic; }

        public ArrayTypeNode(GenericCallNode _generic) : base(_generic)
        {
            generic = _generic;
        }

        public override string ToString() => $"Array Type Node";
    }
}
