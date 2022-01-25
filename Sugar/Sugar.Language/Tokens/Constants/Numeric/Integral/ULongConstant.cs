using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Constants.Numeric.Integral
{
    internal sealed class ULongConstant : IntegralConstant
    {
        public override IntegerType IntegerType => IntegerType.ULong;

        public ULongConstant(string _value, int _index) : base(_value, _index)
        {

        }
    }
}
