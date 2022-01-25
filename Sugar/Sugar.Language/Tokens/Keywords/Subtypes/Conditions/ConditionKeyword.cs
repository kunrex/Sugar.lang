using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Keywords.Subtypes.Conditions
{
    internal sealed partial class ConditionKeyword : Keyword
    {
        public override KeywordType KeywordType => KeywordType.Condition;

        private ConditionKeyword(string _value, byte _id) : base(_value, _id)
        {

        }

        private ConditionKeyword() : base()
        {

        }

        public override Token Clone() => new ConditionKeyword()
        {
            Value = Value,
            Id = Id
        };
    }
}
