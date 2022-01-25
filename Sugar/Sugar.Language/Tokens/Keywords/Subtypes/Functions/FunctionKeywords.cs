using System;
using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Keywords.Subtypes.Functions
{
    internal sealed partial class FunctionKeyword : Keyword
    {
        public static readonly FunctionKeyword Void = new FunctionKeyword("void", 0);
        public static readonly FunctionKeyword Constructor = new FunctionKeyword("constructor", 1);

        public static readonly FunctionKeyword Operator = new FunctionKeyword("operator", 2);
        public static readonly FunctionKeyword Explicit = new FunctionKeyword("explicit", 3);
        public static readonly FunctionKeyword Implicit = new FunctionKeyword("implicit", 4);
    }
}
