using System;

namespace Sugar.Language.Tokens.Enums
{
    [Flags]
    internal enum TokenType : ushort
    {
        Identifier = 1,
        Constant = 2,
        Keyword = 4,
        Seperator = 8,
        UnaryOperator = 16,
        BinaryOperator = 32,
        AssignmentOperator = 64,

        Invalid = 128,
        Operator = UnaryOperator | BinaryOperator | AssignmentOperator,
    }
}
