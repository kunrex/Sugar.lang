using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.SyntaxCollections
{
    internal sealed partial class SyntaxCollection
    {
        public static SyntaxCollection VariableParseError = new SyntaxCollection(SyntaxKind.Equals, SyntaxKind.Comma, SyntaxKind.Semicolon);
    }
}
