using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Keywords.Subtypes.Types
{
    internal sealed partial class TypeKeyword : Keyword
    {
        public override KeywordType KeywordType { get => KeywordType.Type; }

        private TypeKeyword(string _value, SyntaxKind _syntaxKind) : base(_value, _syntaxKind)
        {

        }

        public override Token Clone() => new TypeKeyword(Value, SyntaxKind);
    }
}
