using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Keywords.Subtypes.Loops
{
    internal sealed partial class LoopKeyword : Keyword
    {
        public override KeywordType KeywordType => KeywordType.Loop;

        private LoopKeyword(string _value, byte _id) : base(_value, _id)
        {

        }

        private LoopKeyword() : base()
        {

        }

        public override Token Clone() => new LoopKeyword()
        {
            Value = Value,
            Id = Id
        };
    }
}
