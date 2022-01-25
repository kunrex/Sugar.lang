using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Interfaces.Expressions;

namespace Sugar.Language.Parsing.Nodes.Expressions.Associative
{
    internal sealed class IndexerExpression : ExpressionNode, IBinaryExpression
    {
        public override NodeType NodeType => NodeType.Indexer;

        public Node LHS { get => Children[0]; }
        public Node RHS { get => Children[1]; }

        public IndexerExpression(Node _operhand, Node _arguments) 
        {
            Children = new List<Node>() { _operhand, _arguments };
        }

        public override string ToString() => $"Indexer Expression";
    }
}
