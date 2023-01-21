using System;

namespace Sugar.Language.Tokens.Constants.Numeric.Integral
{
    internal abstract class IntegralConstant : Constant, IValueComparisonToken
    {
        public IntegralConstant(string _value, int _index) : base(_value)
        {
            Index = _index;
        }

        public override bool Equals(Token obj) => ((IValueComparisonToken)this).Equals(obj, Value);
    }
}
