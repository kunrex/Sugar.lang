using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Constants.Numeric.Integral
{
    internal abstract class IntegralConstant : Constant, IValueComparisonToken
    {
        public abstract IntegerType IntegerType { get; }
        public override ConstantType ConstantType => ConstantType.Integer;

        public IntegralConstant(string _value, int _index) : base(_value, 0)
        {
            Index = _index;
        }

        public override bool Equals(Token obj) => ((IValueComparisonToken)this).Equals(obj, Value);
    }
}
