using System;

using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Types.Enums;

namespace Sugar.Language.Parsing.Nodes.Types
{
    internal abstract class TypeNode : ParseNodeCollection
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Type; }

        public abstract TypeNodeEnum Type { get; }

        public TypeNode() : base()
        {

        }

        public TypeNode(params ParseNode[] _children) : base(_children)
        {

        }

        public override ParseNode AddChild(ParseNode node) { return this; }
    }
}
