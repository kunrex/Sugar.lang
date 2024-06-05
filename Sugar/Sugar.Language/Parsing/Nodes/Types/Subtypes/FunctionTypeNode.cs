using System;

using Sugar.Language.Parsing.Nodes.Types.Enums;

using Sugar.Language.Parsing.Nodes.Values.Generics;

namespace Sugar.Language.Parsing.Nodes.Types.Subtypes
{
    internal sealed class FunctionTypeNode : TypeNode
    {
        public override TypeNodeEnum Type { get => TypeNodeEnum.Function; }

        private readonly GenericCallNode generic;
        public GenericCallNode Generic { get => generic; }

        public FunctionTypeNode(GenericCallNode _generic) : base(_generic)
        {
            generic = _generic;
        }

        public override string ToString() => $"Function Type Node";
    }
}
