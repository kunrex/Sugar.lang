using System;
using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Keywords.Subtypes.Functions
{
    internal sealed partial class FunctionKeyword : Keyword
    {
        public static readonly FunctionKeyword Void = new FunctionKeyword("void", 0);
        public static readonly FunctionKeyword Indexer = new FunctionKeyword("indexer", 1);
        public static readonly FunctionKeyword Constructor = new FunctionKeyword("constructor", 2);

        public static readonly FunctionKeyword Operator = new FunctionKeyword("operator", 3);
        public static readonly FunctionKeyword Explicit = new FunctionKeyword("explicit", 4);
        public static readonly FunctionKeyword Implicit = new FunctionKeyword("implicit", 5);
    }
}
