using System;

namespace Sugar.Language.Tokens.Keywords.Subtypes.Conditions
{
    internal sealed partial class ConditionKeyword : Keyword
    {
        public static readonly ConditionKeyword If = new ConditionKeyword("if", 0);
        public static readonly ConditionKeyword Else = new ConditionKeyword("else", 1);
        public static readonly ConditionKeyword Case = new ConditionKeyword("case", 2);
        public static readonly ConditionKeyword Switch = new ConditionKeyword("switch", 3);
    }
}
