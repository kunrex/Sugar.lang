using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Interfaces.Expressions;

namespace Sugar.Language.Parsing.Nodes.Expressions.Lambdas
{
    internal sealed class LambdaStatement : ExpressionNode, IUnaryExpression
    {
        public Node Operhand { get => Children[0]; }
        public override NodeType NodeType => NodeType.Lambda;

        public LambdaStatement(Node _statement)
        {
            Children = new List<Node>() { _statement };
        }

        public override string ToString() => $"Lambda Statement";
    }
}
