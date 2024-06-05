using System;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Expressions.Operative
{
    internal sealed class TernaryExpression : ExpressionNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Ternary; }

        private readonly ParseNodeCollection condition;
        public ParseNodeCollection Condition { get => condition; }

        private readonly ParseNodeCollection trueExpression;
        public ParseNodeCollection TrueExpression { get => trueExpression; }

        private readonly ParseNodeCollection falseExpression;
        public ParseNodeCollection FalseExpression { get => falseExpression; }

        public TernaryExpression(ParseNodeCollection _condition, ParseNodeCollection _trueExpression, ParseNodeCollection _falseExpression) : base(_condition, _trueExpression, _falseExpression)
        {
            condition = _condition;

            trueExpression = _trueExpression;
            falseExpression = _falseExpression;
        }

        public override string ToString() => $"Ternary Expression Node";
    }
}
