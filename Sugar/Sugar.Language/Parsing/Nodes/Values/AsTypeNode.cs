using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Types;

namespace Sugar.Language.Parsing.Nodes.Values
{
    internal sealed class AsTypeNode : ParseNodeCollection
    {
        public override ParseNodeType NodeType { get => ParseNodeType.AsType; }

        private readonly TypeNode type;
        public TypeNode Type { get => type; }

        public AsTypeNode(TypeNode _type) : base(_type)
        {
            type = _type;
        }

        public override ParseNode AddChild(ParseNode _node) { return this; }

        public override string ToString() => $"As Type Node";
    }
}
