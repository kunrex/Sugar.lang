using System;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.CtrlStatements
{
    internal sealed class ReturnKeyword : ControlStatement
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Return; }

        private readonly ParseNodeCollection child;
        public ParseNodeCollection Child { get => child; }

        public ReturnKeyword() : base()
        {
            child = null;
        }

        public ReturnKeyword(ParseNodeCollection _child) : base(_child)
        {
            child = _child;
        }

        public override string ToString() => $"Return Node";
    }
}
