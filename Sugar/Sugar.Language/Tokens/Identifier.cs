using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens
{
    internal class Identifier : Token, IValueComparisonToken
    {
        public override TokenType Type { get => TokenType.Identifier; }

        public Identifier(string _value, int _index) : base(_value, SyntaxKind.Variable)
        {
            Index = _index;
        }

        public override Token Clone() => throw new NotImplementedException();

        public override bool Equals(Token obj) => ((IValueComparisonToken)this).Equals(obj, Value);
    }
}
