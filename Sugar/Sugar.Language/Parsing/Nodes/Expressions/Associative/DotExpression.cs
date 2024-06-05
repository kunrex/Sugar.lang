using System;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Expressions.Associative
{
    internal sealed class DotExpression : BaseBinaryNode<ParseNodeCollection, ParseNodeCollection>
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Dot; }

        public DotExpression(ParseNodeCollection _lhs, ParseNodeCollection _rhs) : base(_lhs, _rhs)
        {
            
        }

        public override string ToString() => $"Dot Expression";
    }
}
