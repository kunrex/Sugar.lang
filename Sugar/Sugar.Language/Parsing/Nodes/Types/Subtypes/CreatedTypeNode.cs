using System;

using Sugar.Language.Parsing.Nodes.Types.Enums;

using Sugar.Language.Parsing.Nodes.Values.Generics;

namespace Sugar.Language.Parsing.Nodes.Types.Subtypes
{
    internal sealed class CreatedTypeNode : TypeNode
    {
        public override TypeNodeEnum Type { get => TypeNodeEnum.Created; }

        private readonly ParseNodeCollection identifier;
        public ParseNodeCollection Identifier { get => identifier; }

        private readonly GenericCallNode generic;
        public GenericCallNode Generic { get => generic; }

        public CreatedTypeNode(ParseNodeCollection _identifier) : base(_identifier)
        {
            identifier = _identifier;
            generic = null;
        }

        public CreatedTypeNode(ParseNodeCollection _identifier, GenericCallNode _generic) : base(_identifier, _generic)
        {
            identifier = _identifier;
            generic = _generic;
        }

        public override string ToString() => $"Created Type Node";
    }
}
