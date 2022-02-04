using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Seperators
{
    internal partial class Seperator : Token
    {
        public static readonly Seperator Colon = new Seperator(":", SeperatorKind.Colon);
        public static readonly Seperator Semicolon = new Seperator(";", SeperatorKind.Semicolon);

        public static readonly Seperator Dot = new Seperator(".", SeperatorKind.Dot);
        public static readonly Seperator Comma = new Seperator(",", SeperatorKind.Comma);
        public static readonly Seperator Lambda = new Seperator("=>", SeperatorKind.Lambda);
        public static readonly Seperator Questionmark = new Seperator("?", SeperatorKind.QuestionMark);

        public static readonly Seperator OpenBracket = new Seperator("(", SeperatorKind.OpenBracket);
        public static readonly Seperator CloseBracket = new Seperator(")", SeperatorKind.CloseBracket);

        public static readonly Seperator BoxOpenBracket = new Seperator("[", SeperatorKind.BoxOpenBracket);
        public static readonly Seperator BoxCloseBracket = new Seperator("]", SeperatorKind.BoxCloseBracket);

        public static readonly Seperator FlowerOpenBracket = new Seperator("{", SeperatorKind.FlowerOpenBracket);
        public static readonly Seperator FlowerCloseBracket = new Seperator("}", SeperatorKind.FlowerCloseBracket);
    }
}
