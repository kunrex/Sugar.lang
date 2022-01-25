using System;

namespace Sugar.Language.Tokens.Enums
{
    internal enum OperatorType
    {
        Addition,
        Subtraction,
        Multiplication,
        Division,
        Modulus,

        Increment,
        Decrement,

        Not,
        Minus,
        Assignment,

        Equals,
        NotEquals,
        GreaterThan,
        LesserThan,
        GreaterThanEquals,
        LesserThanEquals,

        Or,
        And,

        Dot,
        AsCast,

        BitwiseAnd,
        BitwiseOr,
        BitwiseXor,
        RightShit,
        LeftShift,
        BitwiseNot
    }
}
