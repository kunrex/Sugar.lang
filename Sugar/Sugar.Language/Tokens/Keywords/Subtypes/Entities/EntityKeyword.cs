using System;
using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Keywords.Subtypes.Entities
{
    internal sealed partial class EntityKeyword : Keyword
    {
        public override KeywordType KeywordType => KeywordType.Entity;

        private EntityKeyword(string _value, byte _id) : base(_value, _id)
        {

        }

        private EntityKeyword() : base()
        {

        }

        public override Token Clone() => new EntityKeyword()
        {
            Value = Value,
            Id = Id
        };
    }
}
