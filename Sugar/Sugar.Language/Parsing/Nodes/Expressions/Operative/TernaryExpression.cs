using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Expressions.Operative
{
    internal sealed class TernaryExpression : ExpressionNode
    {
        public override NodeType NodeType => NodeType.Ternary;

        public Node Condition { get => Children[0]; }
        public Node TrueExpression { get => Children[1]; }
        public Node FalseExpression { get => Children[2]; }

        public TernaryExpression(Node _condition, Node _trueExpression, Node _falseExpression)
        {
            Children = new List<Node>() { _condition, _trueExpression, _falseExpression };
        }

        public override string ToString() => $"Ternary Expression Node";
    }
}
