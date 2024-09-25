using System;

namespace Sugar.Language.Tokens.Enums
{
    [Flags]
    internal enum SeperatorKind : ushort
    {
        None = 0,

        Colon = 1,
        Semicolon = 2,

        Dot = 4,
        Comma = 8,
        Lambda = 16,
        QuestionMark = 32,

        OpenBracket = 64,
        CloseBracket = 128,

        BoxOpenBracket = 256,
        BoxCloseBracket = 512,

        FlowerOpenBracket = 1024,
        FlowerCloseBracket = 2048,
    }
}
