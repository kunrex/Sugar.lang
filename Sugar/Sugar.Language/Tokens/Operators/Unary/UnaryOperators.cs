using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Operators.Unary
{
    internal sealed partial class UnaryOperator : Operator
    {
        public static readonly UnaryOperator Increment = new UnaryOperator("++", OperatorKind.Increment, true, 0);
        public static readonly UnaryOperator Decrement = new UnaryOperator("--", OperatorKind.Decrement, true, 0);

        public static readonly UnaryOperator IncrementPrefix = new UnaryOperator("++", OperatorKind.Increment, false, 1);
        public static readonly UnaryOperator DecrementPrefix = new UnaryOperator("--", OperatorKind.Decrement, false, 1);

        public static readonly UnaryOperator Not = new UnaryOperator("!", OperatorKind.Not, false, 2);
        public static readonly UnaryOperator Minus = new UnaryOperator("-", OperatorKind.Minus, false, 2);
        public static readonly UnaryOperator BitwiseNot = new UnaryOperator("~", OperatorKind.BitwiseNot, false, 2);
    }
}
