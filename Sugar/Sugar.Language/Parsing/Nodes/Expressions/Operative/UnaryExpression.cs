using System;
using System.Collections.Generic;

using Sugar.Language.Tokens.Operators;
using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Tokens.Operators.Unary;
using Sugar.Language.Parsing.Nodes.Interfaces.Expressions;

namespace Sugar.Language.Parsing.Nodes.Expressions.Operative
{
    internal sealed class UnaryExpression : ExpressionNode, IUnaryExpression
    {
        public Operator Operator { get; private set; }
        public override NodeType NodeType => NodeType.Unary;

        public Node Operhand { get => Children[0]; }

        public UnaryExpression(Operator _operator, Node _opperhand)
        {
            Operator = _operator;
            Children = new List<Node>() { _opperhand };
        }

        public override string ToString() => $"Unary Expression [Operator: {Operator.Value} Prefix: {!(Operator == UnaryOperator.Decrement || Operator == UnaryOperator.Increment)}]";
    }
}
