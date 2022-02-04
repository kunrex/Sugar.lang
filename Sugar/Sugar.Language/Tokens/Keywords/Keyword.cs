using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Keywords
{
    internal partial class Keyword : Token
    {
        public override TokenType Type { get => TokenType.Keyword; }

        public virtual KeywordType KeywordType { get => KeywordType.General; }

        protected Keyword(string _value, SyntaxKind _syntaxKind) : base(_value, _syntaxKind)
        {
            
        }

        public override Token Clone() => new Keyword(Value, SyntaxKind);
    }
}
