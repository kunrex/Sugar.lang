using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Constants.Numeric.Integral
{
    internal sealed class UIntegerConstant : IntegralConstant
    {
        public override IntegerType IntegerType => IntegerType.UInt;

        public UIntegerConstant(string _value, int _index) : base(_value, _index)
        {

        }
    }
}
