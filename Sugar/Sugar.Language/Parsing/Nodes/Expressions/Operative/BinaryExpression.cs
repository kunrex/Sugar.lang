using System;

using Sugar.Language.Tokens.Operators;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Expressions.Operative
{
    internal sealed class BinaryExpression : BaseBinaryNode<ParseNodeCollection, ParseNodeCollection>
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Binary; }

        private readonly Operator binaryOperator;
        public Operator Operator { get => binaryOperator; }

        public BinaryExpression(Operator _operator, ParseNodeCollection _lhs, ParseNodeCollection _rhs) : base(_lhs, _rhs)
        {
            binaryOperator = _operator;
        }

        public override string ToString() => $"Binary Expression [Operator: {Operator.Value}]";
    }
}
