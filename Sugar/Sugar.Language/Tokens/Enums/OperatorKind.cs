using System;

namespace Sugar.Language.Tokens.Enums
{
    internal enum OperatorKind : byte
    {
        Addition = 73,
        Subtraction = 74,
        Multiplication = 75,
        Division = 76,
        Modulus = 77,

        BitwiseAnd = 78,
        BitwiseOr = 79,
        BitwiseXor = 80,
        RightShit = 81,
        LeftShift = 82,
        BitwiseNot = 83,

        Increment = 84,
        Decrement = 85,

        Not = 86,
        Minus = 87,

        Equals = 88,
        NotEquals = 89,
        GreaterThan = 90,
        LesserThan = 91,
        GreaterThanEquals = 92,
        LesserThanEquals = 93,

        Or = 94,
        And = 95,

        Dot = 96,
        AsCast = 97,

        Assignment = 98,

        AssignmentAddition = 99,
        AssignmentSubtract = 100,
        AssignmentMultiplication = 101,
        AssignmentDivision = 102,
        AssignmentModulus = 103,

        AssignmentBitwiseAnd = 104,
        AssignmentBitwiseOr = 105,
        AssignmentBitwiseXor = 106,
        AssignmentBitwiseRighshift = 107,
        AssignmentBitwiseLeftShift = 108,

        Plus = 114
    }
}
