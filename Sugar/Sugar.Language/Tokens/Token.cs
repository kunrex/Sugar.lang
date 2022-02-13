using System;


using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens
{
    internal abstract class Token
    {
        public int Index { get; protected set; }
        public string Value { get; protected set; }

        public abstract TokenType Type { get; }
        public SyntaxKind SyntaxKind { get; protected set; }

        public Token(string _value, SyntaxKind _syntaxKind)
        {
            Value = _value;
            SyntaxKind = _syntaxKind;
        }

        protected Token()
        {

        }

        public override string ToString() => $"[{Type} : {Value}]";

        public abstract Token Clone();

        public Token Clone(int _index)
        {
            var copy = Clone();
            copy.Index = _index;

            return copy;
        }

        public static bool operator ==(Token a, Token b)
        {
            if (a is null)
            {
                if (b is null)
                    return true;

                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(Token a, Token b) => !(a == b);

        public override bool Equals(object obj) => Equals((Token)obj);

        public virtual bool Equals(Token obj)
        {
            if (obj is null)
                return false;

            return SyntaxKind == obj.SyntaxKind;
        }

        public override int GetHashCode() => base.GetHashCode();
    }
}
