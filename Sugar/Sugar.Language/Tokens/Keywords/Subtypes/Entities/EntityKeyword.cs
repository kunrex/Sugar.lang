using System;
using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Keywords.Subtypes.Entities
{
    internal sealed partial class EntityKeyword : Keyword
    {
        public override KeywordType KeywordType { get => KeywordType.Entity; }

        private EntityKeyword(string _value, SyntaxKind _syntaxKind) : base(_value, _syntaxKind)
        {

        }

        public override Token Clone() => new EntityKeyword(Value, SyntaxKind);
    }
}
