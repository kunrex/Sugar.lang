using System;

namespace Sugar.Language.Tokens.Enums
{
    [Flags]
    internal enum SeperatorKind : ushort
    {
        Any = 0,
        Colon = 4,
        Semicolon = 8,

        Dot = 16,
        Comma = 32,
        Lambda = 64,
        QuestionMark = 128,

        OpenBracket = 256,
        CloseBracket = 512,

        BoxOpenBracket = 2024,
        BoxCloseBracket = 4096,

        FlowerOpenBracket = 8192,
        FlowerCloseBracket = 16384,
    }
}
