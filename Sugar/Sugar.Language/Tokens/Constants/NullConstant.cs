using System;
using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Constants
{
    internal sealed class NullConstant : Constant
    {
        public static readonly NullConstant Null = new NullConstant("null");

        public override ConstantType ConstantType => ConstantType.Null;

        private NullConstant(string _value) : base(_value, 0)
        {

        }

        private NullConstant()
        {

        }

        public override Token Clone() => new NullConstant()
        {
            Value = Value,
            Id = Id
        };
    }
}
