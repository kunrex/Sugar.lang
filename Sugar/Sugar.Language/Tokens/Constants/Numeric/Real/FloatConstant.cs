using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Constants.Numeric.Real
{
    internal sealed class FloatConstant : RealConstant
    {
        public override RealType RealType => RealType.Float;

        public FloatConstant(string _value, int _index) : base(_value, _index)
        {

        }
    }
}
