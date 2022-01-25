﻿using System;
using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Constants
{
    internal sealed class CharacterConstant : Constant, IValueComparisonToken
    {
        public override ConstantType ConstantType => ConstantType.Char;

        public CharacterConstant(string _value, int _index) : base(_value, 0)
        {
            Index = _index;
            Value = _value[0].ToString();
        }

        public override bool Equals(Token obj) => ((IValueComparisonToken)this).Equals(obj, Value);
    }
}