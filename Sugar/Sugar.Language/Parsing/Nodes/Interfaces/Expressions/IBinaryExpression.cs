using System;

namespace Sugar.Language.Parsing.Nodes.Interfaces.Expressions
{
    internal interface IBinaryExpression<LHSType, RHSType> where LHSType : ParseNodeCollection where RHSType : ParseNodeCollection
    {
        public LHSType LHS { get; }
        public RHSType RHS { get; }
    }
}
