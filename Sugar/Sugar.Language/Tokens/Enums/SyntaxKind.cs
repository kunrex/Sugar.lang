using System;

namespace Sugar.Language.Tokens.Enums
{
    internal enum SyntaxKind : ushort
    {
        Constant = 0,
        Variable = 3,

        Input = 5,
        Print = 6,

        Colon = SeperatorKind.Colon,

        Import = 7,

        Throw = 9,
        Create = 10,

        Semicolon = SeperatorKind.Semicolon,

        Var = 11,
        Default = 12,

        This = 13,

        Get = 14,
        Set = 15,

        When = 17,

        Dot = SeperatorKind.Dot,

        Try = 18,
        Catch = 19,
        Finally = 20,

        Object = 21,

        SByte = 22,
        Byte = 23,

        Short = 24,
        UShort = 25,

        Int = 26,
        UInt = 27,

        Long = 28,
        Ulong = 29,

        Float = 30,
        Double = 31,
        Decimal = 33,

        Comma = SeperatorKind.Comma,

        Bool = 34,

        Char = 35,
        String = 36,

        Array = 37,

        Enum = 38,
        Class = 39,
        Struct = 40,
        Interface = 41,
        Namespace = 42,

        If = 43,
        Else = 44,
        Case = 45,
        Switch = 46,

        Break = 47,
        Return = 48,
        Continue = 49,

        Static = 50,

        Public = 51,
        Private = 52,
        Protected = 53,

        Abstract = 54,
        Override = 55,

        Sealed = 56,
        Virtual = 57,

        Const = 58,
        Readonly = 59,

        In = 60,
        Out = 61,
        Ref = 62,

        Operator = 63,
        Explicit = 65,
        Implicit = 66,

        Lambda = SeperatorKind.Lambda,

        Void = 67,
        Indexer = 68,
        Constructor = 69,

        True = 71,
        False = 71,

        Null = 72,

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

        Do = 109,
        For = 110,
        While = 111,
        Foreach = 112,

        AsType = 113,

        Plus = OperatorKind.Plus,

        Parent = 115,

        Action = 116,
        Function = 117,

        QuestionMark = SeperatorKind.QuestionMark,

        OpenBracket = SeperatorKind.OpenBracket,
        CloseBracket = SeperatorKind.CloseBracket,

        BoxOpenBracket = SeperatorKind.BoxOpenBracket,
        BoxCloseBracket = SeperatorKind.BoxCloseBracket,

        FlowerOpenBracket = SeperatorKind.FlowerOpenBracket,
        FlowerCloseBracket = SeperatorKind.FlowerCloseBracket,

        Invalid = 118
    }
}
