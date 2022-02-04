using System;
using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Seperators
{
    internal partial class Seperator : Token
    {
        public override TokenType Type { get => TokenType.Seperator; }

        public SeperatorKind SeperatorKind { get; private set; }

        private Seperator(string _value, SeperatorKind _type) : base(_value, (SyntaxKind)_type)
        {
            SeperatorKind = _type;
        }

        private Seperator(string _value, SyntaxKind _type, SeperatorKind _seperatorType) : base(_value, _type)
        {
            SeperatorKind = _seperatorType;
        }

        public override Token Clone() => new Seperator(Value, SyntaxKind, SeperatorKind);
    }
}
