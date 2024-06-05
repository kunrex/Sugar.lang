using System;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Values
{
    internal sealed class ThisNode : ParseNodeCollection
    {
        public override ParseNodeType NodeType { get => ParseNodeType.This; }

        private readonly ParseNodeCollection reference;
        public ParseNodeCollection Reference { get => reference; }

        public ThisNode(ParseNodeCollection _reference) : base(_reference)
        {
            reference = _reference;
        }

        public override ParseNode AddChild(ParseNode _node) { return this; }

        public override string ToString() => $"This Node";
    }
}
