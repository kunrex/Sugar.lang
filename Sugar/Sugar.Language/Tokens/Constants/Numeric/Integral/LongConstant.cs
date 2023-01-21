using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Constants.Numeric.Integral
{
    internal sealed class LongConstant : IntegralConstant
    {
        public override ConstantType ConstantType => ConstantType.Long;

        public LongConstant(string _value, int _index) : base(_value, _index)
        {

        }
    }
}
