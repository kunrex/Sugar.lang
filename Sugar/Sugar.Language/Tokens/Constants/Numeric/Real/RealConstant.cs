using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Constants.Numeric.Real
{
    internal abstract class RealConstant : Constant, IValueComparisonToken
    {
        public abstract RealType RealType { get; }
        public override ConstantType ConstantType => ConstantType.Real;

        public RealConstant(string _value, int _index) : base(_value, 0)
        {
            Index = _index;
        }

        public override bool Equals(Token obj) => ((IValueComparisonToken)this).Equals(obj, Value);
    }
}
