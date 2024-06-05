using System;

namespace Sugar.Language.Tokens.Enums
{
    [Flags]
    internal enum SeperatorKind : ushort
    {
        None = 0,

        Colon = 2,
        Semicolon = 4,

        Dot = 8,
        Comma = 16,
        Lambda = 32,
        QuestionMark = 64,

        OpenBracket = 128,
        CloseBracket = 256,

        BoxOpenBracket = 512,
        BoxCloseBracket = 1024,

        FlowerOpenBracket = 2048,
        FlowerCloseBracket = 5096,
    }
}
