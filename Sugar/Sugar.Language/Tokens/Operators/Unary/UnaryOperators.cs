using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Operators.Unary
{
    internal sealed partial class UnaryOperator : Operator
    {
        public static readonly UnaryOperator Increment = new UnaryOperator("++", 0, OperatorType.Increment, true, 0);
        public static readonly UnaryOperator Decrement = new UnaryOperator("--", 1, OperatorType.Decrement, true, 0);

        public static readonly UnaryOperator IncrementPrefix = new UnaryOperator("++", 2, OperatorType.Increment, false, 1);
        public static readonly UnaryOperator DecrementPrefix = new UnaryOperator("--", 3, OperatorType.Decrement, false, 1);

        public static readonly UnaryOperator Not = new UnaryOperator("!", 4, OperatorType.Not, false, 2);
        public static readonly UnaryOperator Minus = new UnaryOperator("-", 5, OperatorType.Minus, false, 2);
        public static readonly UnaryOperator BitwiseNot = new UnaryOperator("~", 6, OperatorType.BitwiseNot, false, 2);
    }
}
