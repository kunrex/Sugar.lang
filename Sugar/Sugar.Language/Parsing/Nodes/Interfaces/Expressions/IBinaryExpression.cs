using System;

namespace Sugar.Language.Parsing.Nodes.Interfaces.Expressions
{
    internal interface IBinaryExpression
    {
        public Node LHS { get; }
        public Node RHS { get; }
    }
}
