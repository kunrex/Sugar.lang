using System;

using Sugar.Language.Tokens.Enums;
using Sugar.Language.Tokens.Operators.Assignment;

namespace Sugar.Language.Tokens.Operators.Binary
{
    internal sealed partial class BinaryOperator : Operator
    {
        public static readonly BinaryOperator Addition = new BinaryOperator("+", 0, OperatorType.Addition, true, 4);
        public static readonly BinaryOperator Subtraction = new BinaryOperator("-", 1, OperatorType.Subtraction, true, 4);

        public static readonly BinaryOperator Multiplication = new BinaryOperator("*", 2, OperatorType.Multiplication, true, 3);
        public static readonly BinaryOperator Division = new BinaryOperator("/", 3, OperatorType.Division, true, 3);
        public static readonly BinaryOperator Modulus = new BinaryOperator("%", 4, OperatorType.Modulus, true, 3);

        public static readonly AssignmentOperator Assignment = new AssignmentOperator(5);

        public static readonly BinaryOperator And = new BinaryOperator("&&", 6, OperatorType.And, true, 11);
        public static readonly BinaryOperator Or = new BinaryOperator("||", 7, OperatorType.Or, true, 12);

        public static readonly BinaryOperator BitwiseAnd = new BinaryOperator("&", 8, OperatorType.BitwiseAnd, true, 8);
        public static readonly BinaryOperator BitwiseOr = new BinaryOperator("|", 9, OperatorType.BitwiseOr, true, 10);
        public static readonly BinaryOperator BitwiseXor = new BinaryOperator("^", 10, OperatorType.BitwiseXor, true, 9);
        public static readonly BinaryOperator RightShift = new BinaryOperator(">>", 11, OperatorType.RightShit, true, 5);
        public static readonly BinaryOperator LeftShift = new BinaryOperator("<<", 12, OperatorType.LeftShift, true, 5);

        public new static readonly BinaryOperator Equals = new BinaryOperator("==", 13, OperatorType.Equals, true, 7);
        public static readonly BinaryOperator NotEquals = new BinaryOperator("!=", 14, OperatorType.NotEquals, true, 7);
        public static readonly BinaryOperator LesserThan = new BinaryOperator("<", 15, OperatorType.LesserThan, true, 6);
        public static readonly BinaryOperator GreaterThan = new BinaryOperator(">", 16, OperatorType.GreaterThan, true, 6);
        public static readonly BinaryOperator LesserThanEquals = new BinaryOperator("<=", 17, OperatorType.LesserThanEquals, true, 6);
        public static readonly BinaryOperator GreaterThanEquals = new BinaryOperator(">=", 18, OperatorType.GreaterThanEquals, true, 6);

        public static readonly BinaryOperator DotOperator = new BinaryOperator(".", 19, OperatorType.Dot, true, 0);
        public static readonly BinaryOperator AsCastOperator = new BinaryOperator("as", 20, OperatorType.AsCast, false, 1);
    }
}
