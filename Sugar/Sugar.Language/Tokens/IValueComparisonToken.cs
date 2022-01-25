using System;
namespace Sugar.Language.Tokens
{
    internal interface IValueComparisonToken
    {
        public bool Equals(Token obj, string value)
        {
            if (obj is null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            return obj.Value == value;
        }
    }
}
