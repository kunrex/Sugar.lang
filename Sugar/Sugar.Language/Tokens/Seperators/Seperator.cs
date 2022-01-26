using System;
using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Seperators
{
    internal partial class Seperator : Token
    {
        public override TokenType Type => TokenType.Seperator;
        public override int SubType => (int)SeperatorType;

        protected override byte TypeID { get => 3; }

        public SeperatorType SeperatorType { get; private set; }

        public Seperator(string _value, byte _id, SeperatorType _type) : base(_value, _id)
        {
            SeperatorType = _type;
        }

        private Seperator()
        {

        }

        public override Token Clone() => new Seperator()
        {
            Value = Value,
            Id = Id,
            SeperatorType = SeperatorType
        };
    }
}
