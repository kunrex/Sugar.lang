﻿using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Constants.Numeric.Real
{
    internal sealed class DecimalConstant : RealConstant
    {
        public override ConstantType ConstantType => ConstantType.Decimal;

        public DecimalConstant(string _value, int _index) : base(_value, _index)
        {

        }
    }
}
