using System;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Statements
{
    internal sealed class AssignmentNode : StatementNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Assignment; }

        private readonly ParseNodeCollection value;
        public ParseNodeCollection Value { get => value; }

        private readonly ParseNodeCollection to;
        public ParseNodeCollection To { get => to; }

        public AssignmentNode(ParseNodeCollection _value, ParseNodeCollection _to) : base(_value, _to)
        {
            value = _value;
            to = _to;
        }

        public override string ToString() => $"Assign Node";
    }
}
