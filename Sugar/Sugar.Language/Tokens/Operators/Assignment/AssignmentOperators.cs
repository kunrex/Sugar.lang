using System;

using Sugar.Language.Tokens.Enums;
using Sugar.Language.Tokens.Operators.Binary;

namespace Sugar.Language.Tokens.Operators.Assignment
{
    internal sealed partial class AssignmentOperator : Operator
    {
        public static readonly AssignmentOperator Assignment = new AssignmentOperator("=", OperatorKind.Assignment);

        public static readonly AssignmentOperator AssignmentAddition = new AssignmentOperator(BinaryOperator.Addition);
        public static readonly AssignmentOperator AssignmentSubtraction = new AssignmentOperator(BinaryOperator.Subtraction);

        public static readonly AssignmentOperator AssignmentMultiplication = new AssignmentOperator(BinaryOperator.Multiplication);
        public static readonly AssignmentOperator AssignmentDivision = new AssignmentOperator(BinaryOperator.Division);
        public static readonly AssignmentOperator AssignmentModulus = new AssignmentOperator(BinaryOperator.Modulus);

        public static readonly AssignmentOperator AssignmentBitwiseAnd = new AssignmentOperator(BinaryOperator.BitwiseAnd);
        public static readonly AssignmentOperator AssignmentBitwiseOr = new AssignmentOperator(BinaryOperator.BitwiseOr);
        public static readonly AssignmentOperator AssignmentBitwiseXor = new AssignmentOperator(BinaryOperator.BitwiseXor);
        public static readonly AssignmentOperator AssignmentRightShift = new AssignmentOperator(BinaryOperator.RightShift);
        public static readonly AssignmentOperator AssignmentLeftShift = new AssignmentOperator(BinaryOperator.LeftShift);
    }
}
