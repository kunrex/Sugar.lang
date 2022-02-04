using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Keywords.Subtypes.Conditions
{
    internal sealed partial class ConditionKeyword : Keyword
    {
        public static readonly ConditionKeyword If = new ConditionKeyword("if", SyntaxKind.If);
        public static readonly ConditionKeyword Else = new ConditionKeyword("else", SyntaxKind.Else);
        public static readonly ConditionKeyword Case = new ConditionKeyword("case", SyntaxKind.Case);
        public static readonly ConditionKeyword Switch = new ConditionKeyword("switch", SyntaxKind.Switch);
    }
}
