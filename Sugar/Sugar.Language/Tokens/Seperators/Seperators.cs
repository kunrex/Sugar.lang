using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Seperators
{
    internal partial class Seperator : Token
    {
        public static readonly Seperator Colon = new Seperator(":", 0, SeperatorType.Colon);
        public static readonly Seperator Semicolon = new Seperator(";", 1, SeperatorType.Semicolon);

        public static readonly Seperator Dot = new Seperator(".", 2, SeperatorType.Dot);
        public static readonly Seperator Comma = new Seperator(",", 3, SeperatorType.Comma);
        public static readonly Seperator Lambda = new Seperator("=>", 4, SeperatorType.Lambda);
        public static readonly Seperator Questionmark = new Seperator("?", 5, SeperatorType.QuestionMark);

        public static readonly Seperator OpenBracket = new Seperator("(", 6, SeperatorType.OpenBracket);
        public static readonly Seperator CloseBracket = new Seperator(")", 7, SeperatorType.CloseBracket);

        public static readonly Seperator BoxOpenBracket = new Seperator("[", 8, SeperatorType.BoxOpenBracket);
        public static readonly Seperator BoxCloseBracket = new Seperator("]", 9, SeperatorType.BoxCloseBracket);

        public static readonly Seperator FlowerOpenBracket = new Seperator("{", 10, SeperatorType.FlowerOpenBracket);
        public static readonly Seperator FlowerCloseBracket = new Seperator("}", 11, SeperatorType.FlowerCloseBracket);
    }
}
