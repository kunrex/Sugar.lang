using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Constants.Numeric.Integral
{
    internal sealed class IntegerConstant : IntegralConstant
    {
        public override ConstantType ConstantType => ConstantType.Integer;

        public IntegerConstant(string _value, int _index) : base(_value, _index)
        {

        }      
    }
}
