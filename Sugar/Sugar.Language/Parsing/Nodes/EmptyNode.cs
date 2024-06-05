using System;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes
{
    internal sealed class EmptyNode : ParseNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Empty; }

        public EmptyNode() : base() { }

        public override void SetParent() { }

        public override string ToString() => $"Empty Node";

        protected override void PrintChildren(string indent) { }
    }
}
