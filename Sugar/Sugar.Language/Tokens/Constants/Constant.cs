using System;
using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Constants
{
    internal abstract class Constant : Token
    {
        public override TokenType Type { get => TokenType.Constant; }
        public override int SubType => (int)ConstantType;

        public abstract ConstantType ConstantType { get; }

        protected override byte TypeID { get => 1; }

        public Constant(string _value, byte _id) : base(_value, _id)
        {
            Id = (ushort)(TypeID * 1000 + ((int)ConstantType) * 100 + _id);
        }

        protected Constant() : base()
        {

        }

        public override Token Clone() => throw new NotImplementedException();
    }
}
