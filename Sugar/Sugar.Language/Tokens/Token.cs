using System;


using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens
{
    internal abstract class Token
    {
        public int Index { get; protected set; }
        public string Value { get; protected set; }

        public abstract TokenType Type { get; }
        public abstract int SubType { get; }

        public ushort UniqueID { get => Id; }

        protected ushort Id { get; set; }
        protected abstract byte TypeID { get; }

        public Token(string _value, byte _id)
        {
            Value = _value;
            Id = (ushort)(TypeID * 100 + _id);
        }

        protected Token()
        {

        }

        public abstract Token Clone();

        public Token Clone(int _index)
        {
            var copy =  Clone();
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

            if (ReferenceEquals(this, obj))
                return true;

            return UniqueID == obj.UniqueID;
        }

        public override int GetHashCode() => base.GetHashCode();
    }
}
