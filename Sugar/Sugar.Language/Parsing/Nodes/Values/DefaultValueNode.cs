using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Types;

namespace Sugar.Language.Parsing.Nodes.Values
{
    internal sealed class DefaultValueNode : ParseNodeCollection
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Default; }

        private readonly TypeNode type;
        public TypeNode Type { get => type; }

        public DefaultValueNode()
        {
            type = null;
        }

        public DefaultValueNode(TypeNode _type) : base(_type)
        {
            type = _type;
        }

        public override ParseNode AddChild(ParseNode _node) { return this; }

        public override string ToString() => $"Default Value Node";
    }
}
