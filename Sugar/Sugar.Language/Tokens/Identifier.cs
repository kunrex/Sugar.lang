using System;
using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens
{
    internal class Identifier : Token, IValueComparisonToken
    {
        public override TokenType Type => TokenType.Identifier;
        protected override byte TypeID { get => 0; }

        public Identifier(string _value, int _index) : base(_value, 0)
        {
            Index = _index;
        }

        public override bool Equals(Token obj) => ((IValueComparisonToken)this).Equals(obj, Value);

        public override Token Clone() => throw new NotImplementedException();
    }
}
