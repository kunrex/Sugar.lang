using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Constants
{
    internal class StringConstant : Constant, IValueComparisonToken
    {
        public override ConstantType ConstantType => ConstantType.String;

        public StringConstant(string _value, int _index) : base(_value, 0)
        {
            Index = _index;
        }

        public override bool Equals(Token obj) => ((IValueComparisonToken)this).Equals(obj, Value);
    }
}
