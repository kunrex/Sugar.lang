using System;
namespace Sugar.Language.Tokens.Seperators
{
    internal partial class Seperator : Token
    {
        public static readonly Seperator Colon = new Seperator(":", 0);
        public static readonly Seperator Semicolon = new Seperator(";", 1);

        public static readonly Seperator Dot = new Seperator(".", 2);
        public static readonly Seperator Comma = new Seperator(",", 3);
        public static readonly Seperator Lambda = new Seperator("=>", 4);
        public static readonly Seperator Questionmark = new Seperator("?", 5);

        public static readonly Seperator OpenBracket = new Seperator("(", 6);
        public static readonly Seperator CloseBracket = new Seperator(")", 7);

        public static readonly Seperator BoxOpenBracket = new Seperator("[", 8);
        public static readonly Seperator BoxCloseBracket = new Seperator("]", 9);

        public static readonly Seperator FlowerOpenBracket = new Seperator("{", 10);
        public static readonly Seperator FlowerCloseBracket = new Seperator("}", 11);
    }
}
