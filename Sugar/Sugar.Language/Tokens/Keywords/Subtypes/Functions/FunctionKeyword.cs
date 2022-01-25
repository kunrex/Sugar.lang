using System;
using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Keywords.Subtypes.Functions
{
    internal sealed partial class FunctionKeyword : Keyword
    {
        public override KeywordType KeywordType { get => KeywordType.Function; }

        public FunctionKeyword(string _value, byte _id) : base(_value, _id)
        {

        }

        private FunctionKeyword() : base()
        {

        }

        public override Token Clone() => new FunctionKeyword()
        {
            Value = Value,
            Id = Id
        };
    }
}
