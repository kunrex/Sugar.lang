using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Keywords
{
    internal partial class Keyword : Token
    {
        public override TokenType Type => TokenType.Keyword;
        public virtual KeywordType KeywordType => KeywordType.General;

        protected override byte TypeID { get => 2; }

        protected Keyword(string _value, byte _id) : base(_value, _id)
        {
            Id = (ushort)(TypeID * 1000 + ((byte)KeywordType * 100) + _id);
        }

        protected Keyword() : base()
        {

        }

        public override Token Clone() => new Keyword()
        {
            Value = Value,
            Id = Id
        };
    }
}
