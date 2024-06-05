using System;

using Sugar.Language.Parsing.Nodes.Interfaces.Expressions;

namespace Sugar.Language.Parsing.Nodes.Expressions
{
    internal abstract class BaseUnaryNode : ExpressionNode, IUnaryExpression
    {
        protected readonly ParseNodeCollection operhand;
        public ParseNodeCollection Operhand { get => operhand; }

        public BaseUnaryNode(ParseNodeCollection _operhand) : base(_operhand)
        {
            operhand = _operhand;
        }
    }
}
