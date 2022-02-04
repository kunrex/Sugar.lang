using System;

using Sugar.Language.Tokens.Enums;
using Sugar.Language.Tokens.Operators.Assignment;

namespace Sugar.Language.Tokens.Operators.Binary
{
    internal sealed partial class BinaryOperator : Operator
    {
        public static readonly BinaryOperator Addition = new BinaryOperator("+", OperatorKind.Addition, true, 4);
        public static readonly BinaryOperator Subtraction = new BinaryOperator("-", OperatorKind.Subtraction, true, 4);

        public static readonly BinaryOperator Multiplication = new BinaryOperator("*", OperatorKind.Multiplication, true, 3);
        public static readonly BinaryOperator Division = new BinaryOperator("/", OperatorKind.Division, true, 3);
        public static readonly BinaryOperator Modulus = new BinaryOperator("%", OperatorKind.Modulus, true, 3);

        public static readonly BinaryOperator And = new BinaryOperator("&&", OperatorKind.And, true, 11);
        public static readonly BinaryOperator Or = new BinaryOperator("||", OperatorKind.Or, true, 12);

        public static readonly BinaryOperator BitwiseAnd = new BinaryOperator("&", OperatorKind.BitwiseAnd, true, 8);
        public static readonly BinaryOperator BitwiseOr = new BinaryOperator("|", OperatorKind.BitwiseOr, true, 10);
        public static readonly BinaryOperator BitwiseXor = new BinaryOperator("^", OperatorKind.BitwiseXor, true, 9);
        public static readonly BinaryOperator RightShift = new BinaryOperator(">>", OperatorKind.RightShit, true, 5);
        public static readonly BinaryOperator LeftShift = new BinaryOperator("<<", OperatorKind.LeftShift, true, 5);

        public new static readonly BinaryOperator Equals = new BinaryOperator("==", OperatorKind.Equals, true, 7);
        public static readonly BinaryOperator NotEquals = new BinaryOperator("!=", OperatorKind.NotEquals, true, 7);
        public static readonly BinaryOperator LesserThan = new BinaryOperator("<", OperatorKind.LesserThan, true, 6);
        public static readonly BinaryOperator GreaterThan = new BinaryOperator(">", OperatorKind.GreaterThan, true, 6);
        public static readonly BinaryOperator LesserThanEquals = new BinaryOperator("<=", OperatorKind.LesserThanEquals, true, 6);
        public static readonly BinaryOperator GreaterThanEquals = new BinaryOperator(">=", OperatorKind.GreaterThanEquals, true, 6);

        public static readonly BinaryOperator DotOperator = new BinaryOperator(".", OperatorKind.Dot, true, 0);
        public static readonly BinaryOperator AsCastOperator = new BinaryOperator("as", OperatorKind.AsCast, false, 1);
    }
}
