using System;

using Sugar.Language.Parsing.Nodes.Interfaces.Expressions;

namespace Sugar.Language.Parsing.Nodes.Expressions
{
    internal abstract class BaseBinaryNode<LHSType, RHSType> : ExpressionNode, IBinaryExpression<LHSType, RHSType> where LHSType : ParseNodeCollection where RHSType : ParseNodeCollection
    {
        protected readonly LHSType lhs;
        public LHSType LHS { get => lhs; }

        protected readonly RHSType rhs;
        public RHSType RHS { get => rhs; }

        public BaseBinaryNode(LHSType _lhs, RHSType _rhs) : base(_lhs, _rhs)
        {
            lhs = _lhs;
            rhs = _rhs;
        }
    }
}
