using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Keywords.Subtypes.Types
{
    internal sealed partial class TypeKeyword : Keyword
    {
        public override KeywordType KeywordType => KeywordType.Type;

        private TypeKeyword(string _value, byte _id) : base(_value, _id)
        {

        }

        private TypeKeyword() : base()
        {

        }

        public override Token Clone() => new TypeKeyword()
        {
            Value = Value,
            Id = Id
        };
    }
}
