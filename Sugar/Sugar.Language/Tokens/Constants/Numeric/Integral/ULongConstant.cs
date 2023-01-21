using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Constants.Numeric.Integral
{
    internal sealed class ULongConstant : IntegralConstant
    {
        public override ConstantType ConstantType => ConstantType.ULong;

        public ULongConstant(string _value, int _index) : base(_value, _index)
        {

        }
    }
}
