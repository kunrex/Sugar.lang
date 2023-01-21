using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Constants
{
    internal sealed class CharacterConstant : Constant, IValueComparisonToken
    {
        public override ConstantType ConstantType => ConstantType.Character;

        public CharacterConstant(string _value, int _index) : base(_value)
        {
            Index = _index;
            Value = _value[0].ToString();
        }

        public override bool Equals(Token obj) => ((IValueComparisonToken)this).Equals(obj, Value);
    }
}
