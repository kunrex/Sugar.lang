using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Keywords.Subtypes.Functions
{
    internal sealed partial class FunctionKeyword : Keyword
    {
        public static readonly FunctionKeyword Void = new FunctionKeyword("void", SyntaxKind.Void);
        public static readonly FunctionKeyword Indexer = new FunctionKeyword("indexer", SyntaxKind.Indexer);
        public static readonly FunctionKeyword Constructor = new FunctionKeyword("constructor", SyntaxKind.Constructor);

        public static readonly FunctionKeyword Operator = new FunctionKeyword("operator", SyntaxKind.Operator);
        public static readonly FunctionKeyword Explicit = new FunctionKeyword("explicit", SyntaxKind.Explicit);
        public static readonly FunctionKeyword Implicit = new FunctionKeyword("implicit", SyntaxKind.Implicit);
    }
}
