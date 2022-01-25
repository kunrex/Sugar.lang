using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Constants.Numeric.Integral
{
    internal sealed class IntegerConstant : IntegralConstant
    {
        public override IntegerType IntegerType => IntegerType.Int;

        public IntegerConstant(string _value, int _index) : base(_value, _index)
        {

        }      
    }
}
