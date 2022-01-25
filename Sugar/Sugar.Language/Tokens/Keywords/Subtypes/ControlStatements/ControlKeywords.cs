using System;
namespace Sugar.Language.Tokens.Keywords.Subtypes.ControlStatements
{
    internal sealed partial class ControlKeyword : Keyword
    {
        public static readonly ControlKeyword Break = new ControlKeyword("break", 0);
        public static readonly ControlKeyword Return = new ControlKeyword("return", 1);
        public static readonly ControlKeyword Continue = new ControlKeyword("continue", 2);
    }
}
