using System;
using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Seperators
{
    internal partial class Seperator : Token
    {
        public override TokenType Type => TokenType.Seperator;

        protected override byte TypeID { get => 3; }

        public Seperator(string _value, byte _id) : base(_value, _id)
        {
        
        }

        private Seperator()
        {

        }

        public override Token Clone() => new Seperator()
        {
            Value = Value,
            Id = Id
        };
    }
}
