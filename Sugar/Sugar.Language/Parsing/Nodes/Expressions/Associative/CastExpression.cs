using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Interfaces.Expressions;

namespace Sugar.Language.Parsing.Nodes.Expressions.Associative
{
    internal sealed class CastExpression : ExpressionNode, IBinaryExpression
    {
        public override NodeType NodeType => NodeType.Cast;

        public Node LHS { get => Children[0]; }
        public Node RHS { get => Children[1]; }

        public CastExpression(Node _operhand, Node _type)
        {
            Children = new List<Node>() { _operhand, _type };
        }

        public override string ToString() => $"Cast Node";
    }
}
