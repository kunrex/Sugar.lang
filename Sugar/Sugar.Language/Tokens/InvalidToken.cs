using System;
using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens
{
    internal sealed class InvalidToken : Token
    {
        public override TokenType Type { get => TokenType.Invalid; }

        private readonly Token expectedRead;
        public Token ExpectedRead { get => expectedRead; }

        public InvalidToken(int _index, Token _expectedRead) : base(_expectedRead.Value, SyntaxKind.Invalid)
        {
            Index = _index;
            expectedRead = _expectedRead;
        }

        public override Token Clone() => throw new NotImplementedException();

        public override bool Equals(Token obj)
        {
            if (obj is null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            return GetType() == obj.GetType();
        }
    }
}
