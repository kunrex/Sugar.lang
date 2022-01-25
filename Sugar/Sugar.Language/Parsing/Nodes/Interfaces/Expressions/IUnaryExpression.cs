using System;

namespace Sugar.Language.Parsing.Nodes.Interfaces.Expressions
{
    internal interface IUnaryExpression
    {
        public Node Operhand { get; }
    }
}
