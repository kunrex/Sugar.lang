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

        BoxOpenBracket = 1024,
        BoxCloseBracket = 2048,

        FlowerOpenBracket = 4096,
        FlowerCloseBracket = 8192,
    }
}
