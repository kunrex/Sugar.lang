using System;

namespace Sugar.Language.Tokens.Enums
{
    [Flags]
    internal enum SeperatorType
    {
        Any = 0,
        Colon = 1,
        Semicolon = 2,

        Dot = 4,
        Comma = 4,
        Lambda = 8,
        QuestionMark = 16,

        OpenBracket = 32,
        CloseBracket = 64,

        BoxOpenBracket = 128,
        BoxCloseBracket = 256,

        FlowerOpenBracket = 512,
        FlowerCloseBracket = 1024,
    }
}
