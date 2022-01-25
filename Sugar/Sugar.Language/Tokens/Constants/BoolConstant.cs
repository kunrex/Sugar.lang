using System;
using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Constants
{
    internal sealed class BoolConstant : Constant
    {
        public static readonly BoolConstant True = new BoolConstant("true", 0);
        public static readonly BoolConstant False = new BoolConstant("false", 1);

        public override ConstantType ConstantType => ConstantType.Bool;

        private BoolConstant(string _value, byte _id) : base(_value, _id)
        {

        }

        private BoolConstant()
        {

        }

        public override Token Clone() => new BoolConstant()
        {
            Value = Value,
            Id = Id
        };
    }
}
