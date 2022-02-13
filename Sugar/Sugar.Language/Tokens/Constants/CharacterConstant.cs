using System;

using Sugar.Language.Tokens.Enums;
using Sugar.Language.Tokens.Constants.Numeric.Real;
using Sugar.Language.Tokens.Constants.Numeric.Integral;
using Sugar.Language.Semantics.Analysis.BuiltInTypes.Enums;

namespace Sugar.Language.Tokens.Constants
{
    internal sealed class CharacterConstant : Constant, IValueComparisonToken
    {
        public override ConstantType ConstantType => ConstantType.Char;

        public CharacterConstant(string _value, int _index) : base(_value)
        {
            Index = _index;
            Value = _value[0].ToString();
        }

        public override bool Equals(Token obj) => ((IValueComparisonToken)this).Equals(obj, Value);
    }
}
