using System;

using Sugar.Language.Tokens.Operators;
using Sugar.Language.Tokens.Operators.Unary;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Expressions.Operative
{
    internal sealed class UnaryExpression : BaseUnaryNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Unary; }

        private readonly Operator unaryOperator;
        public Operator Operator { get => unaryOperator; }

        public UnaryExpression(Operator _operator, ParseNodeCollection _opperhand) : base(_opperhand)
        {
            unaryOperator = _operator;
        }

        public override string ToString() => $"Unary Expression [Operator: {Operator.Value} Prefix: {!(Operator == UnaryOperator.Decrement || Operator == UnaryOperator.Increment)}]";
    }
}
