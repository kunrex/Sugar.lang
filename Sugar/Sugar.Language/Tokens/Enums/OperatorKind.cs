using System;

namespace Sugar.Language.Tokens.Enums
{
    internal enum OperatorKind : byte
    {
        Addition = 72,
        Subtraction = 73,
        Multiplication = 74,
        Division = 75,
        Modulus = 76,

        BitwiseAnd = 77,
        BitwiseOr = 78,
        BitwiseXor = 79,
        RightShit = 80,
        LeftShift = 81,
        BitwiseNot = 82,

        Increment = 83,
        Decrement = 84,

        Not = 85,
        Minus = 86,

        Equals = 87,
        NotEquals = 88,
        GreaterThan = 89,
        LesserThan = 90,
        GreaterThanEquals = 91,
        LesserThanEquals = 92,

        Or = 93,
        And = 94,

        Dot = 95,
        AsCast = 96,

        Assignment = 97,

        AssignmentAddition = 98,
        AssignmentSubtract = 99,
        AssignmentMultiplication = 100,
        AssignmentDivision = 101,
        AssignmentModulus = 102,

        AssignmentBitwiseAnd = 103,
        AssignmentBitwiseOr = 104,
        AssignmentBitwiseXor = 105,
        AssignmentBitwiseRighshift = 106,
        AssignmentBitwiseLeftShift = 107
    }
}
