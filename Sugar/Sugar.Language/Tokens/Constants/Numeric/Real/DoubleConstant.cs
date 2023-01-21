using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Constants.Numeric.Real
{
    internal sealed class DoubleConstant : RealConstant
    {
        public override ConstantType ConstantType => ConstantType.Double;

        public DoubleConstant(string _value, int _index) : base(_value, _index)
        {

        }
    }
}
