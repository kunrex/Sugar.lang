using System;

namespace Sugar.Language.Tokens.Constants.Numeric.Real
{
    internal abstract class RealConstant : Constant, IValueComparisonToken
    {
        public RealConstant(string _value, int _index) : base(_value)
        {
            Index = _index;
        }

        public override bool Equals(Token obj) => ((IValueComparisonToken)this).Equals(obj, Value);
    }
}
