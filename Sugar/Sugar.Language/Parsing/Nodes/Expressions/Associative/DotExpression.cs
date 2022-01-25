using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Interfaces.Expressions;

namespace Sugar.Language.Parsing.Nodes.Expressions.Associative
{
    internal sealed class DotExpression : ExpressionNode, IBinaryExpression
    {
        public override NodeType NodeType => NodeType.Dot;

        public Node LHS { get => Children[0]; }
        public Node RHS { get => Children[1]; }

        public DotExpression(Node _lhs, Node _rhs) 
        {
            Children = new List<Node>() { _lhs, _rhs };
        }

        public override string ToString() => $"Dot Expression";
    }
}
