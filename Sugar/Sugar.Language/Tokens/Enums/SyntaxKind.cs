using System;

namespace Sugar.Language.Tokens.Enums
{
    internal enum SyntaxKind : ushort
    {
        Constant = 1,
        Variable = 2,

        Input = 3,
        Print = 5,

        Colon = SeperatorKind.Colon,

        Import = 6,

        Throw = 7,
        Create = 9,

        Semicolon = SeperatorKind.Semicolon,

        Var = 10,
        Default = 11,

        This = 12,

        Get = 13,
        Set = 14,

        When = 15,

        Dot = SeperatorKind.Dot,

        Try = 17,
        Catch = 18,
        Finally = 19,

        Object = 20,

        SByte = 21,
        Byte = 22,

        Short = 23,
        UShort = 24,

        Int = 25,
        UInt = 26,

        Long = 27,
        Ulong = 28,

        Float = 29,
        Double = 30,
        Decimal = 31,

        Comma = SeperatorKind.Comma,

        Bool = 33,

        Char = 34,
        String = 35,

        Array = 36,

        Enum = 37,
        Class = 38,
        Struct = 39,
        Interface = 40,
        Namespace = 41,

        If = 42,
        Else = 43,
        Case = 44,
        Switch = 45,

        Break = 46,
        Return = 57,
        Continue = 48,

        Static = 49,

        Public = 50,
        Private = 51,
        Protected = 52,

        Abstract = 53,
        Override = 54,

        Sealed = 55,
        Virtual = 56,

        Const = 57,
        Readonly = 58,

        In = 59,
        Out = 60,
        Ref = 61,

        Operator = 62,
        Explicit = 63,
        Implicit = 65,

        Lambda = SeperatorKind.Lambda,

        Void = 66,
        Indexer = 67,
        Constructor = 68,

        True = 69,
        False = 70,

        Null = 71,

        Addition = OperatorKind.Addition,
        Subtraction = OperatorKind.Subtraction,
        Multiplication = OperatorKind.Multiplication,
        Division = OperatorKind.Division,
        Modulus = OperatorKind.Modulus,

        BitwiseAnd = OperatorKind.BitwiseAnd,
        BitwiseOr = OperatorKind.BitwiseOr,
        BitwiseXor = OperatorKind.BitwiseXor,
        RightShit = OperatorKind.RightShit,
        LeftShift = OperatorKind.LeftShift,
        BitwiseNot = OperatorKind.BitwiseNot,

        Increment = OperatorKind.Increment,
        Decrement = OperatorKind.Decrement,

        Not = OperatorKind.Not,
        Minus = OperatorKind.Minus,

        Equals = OperatorKind.Equals,
        NotEquals = OperatorKind.NotEquals,
        GreaterThan = OperatorKind.GreaterThan,
        LesserThan = OperatorKind.LesserThan,
        GreaterThanEquals = OperatorKind.GreaterThanEquals,
        LesserThanEquals = OperatorKind.LesserThanEquals,

        Or = OperatorKind.Or,
        And = OperatorKind.And,

        DotOperator = OperatorKind.Dot,
        AsCast = OperatorKind.AsCast,

        Assignment = OperatorKind.Assignment,

        AssignmentAddition = OperatorKind.AssignmentAddition,
        AssignmentSubtract = OperatorKind.AssignmentSubtract,
        AssignmentMultiplication = OperatorKind.Multiplication,
        AssignmentDivision = OperatorKind.AssignmentDivision,
        AssignmentModulus = OperatorKind.AssignmentModulus,

        AssignmentBitwiseAnd = OperatorKind.AssignmentBitwiseAnd,
        AssignmentBitwiseOr = OperatorKind.AssignmentBitwiseOr,
        AssignmentBitwiseXor = OperatorKind.AssignmentBitwiseXor,
        AssignmentBitwiseRighshift = OperatorKind.AssignmentBitwiseRighshift,
        AssignmentBitwiseLeftShift = OperatorKind.AssignmentBitwiseLeftShift,

        Do = 108,
        For = 109,
        While = 110,
        Foreach = 111,

        AsType = 112,

        Plus = OperatorKind.Plus,

        Parent = 114,

        QuestionMark = SeperatorKind.QuestionMark,

        OpenBracket = SeperatorKind.OpenBracket,
        CloseBracket = SeperatorKind.CloseBracket,

        BoxOpenBracket = SeperatorKind.BoxOpenBracket,
        BoxCloseBracket = SeperatorKind.BoxCloseBracket,

        FlowerOpenBracket = SeperatorKind.FlowerOpenBracket,
        FlowerCloseBracket = SeperatorKind.FlowerCloseBracket,
    }
}
