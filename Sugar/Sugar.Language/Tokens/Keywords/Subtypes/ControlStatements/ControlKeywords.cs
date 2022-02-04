using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Keywords.Subtypes.ControlStatements
{
    internal sealed partial class ControlKeyword : Keyword
    {
        public static readonly ControlKeyword Break = new ControlKeyword("break", SyntaxKind.Break);
        public static readonly ControlKeyword Return = new ControlKeyword("return", SyntaxKind.Return);
        public static readonly ControlKeyword Continue = new ControlKeyword("continue", SyntaxKind.Continue);
    }
}
