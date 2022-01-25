using System;
using System.Collections.Generic;

using Sugar.Language.Tokens.Operators;
using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Interfaces.Expressions;

namespace Sugar.Language.Parsing.Nodes.Expressions.Operative
{
    internal sealed class BinaryExpression : ExpressionNode, IBinaryExpression
    {
        public Operator Operator { get; private set; }
        public override NodeType NodeType => NodeType.Binary; 

        public Node LHS { get => Children[0]; }
        public Node RHS { get => Children[1]; }

        public BinaryExpression(Operator _operator, Node _lhs, Node _rhs) 
        {
            Operator = _operator;
            Children = new List<Node>() { _lhs, _rhs };
        }

        public override string ToString() => $"Binary Expression [Operator: {Operator.Value}]";
    }
}
