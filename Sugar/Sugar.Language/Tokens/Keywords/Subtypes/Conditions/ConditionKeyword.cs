using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Keywords.Subtypes.Conditions
{
    internal sealed partial class ConditionKeyword : Keyword
    {
        public override KeywordType KeywordType { get => KeywordType.Condition; }

        private ConditionKeyword(string _value, SyntaxKind _syntaxKind) : base(_value, _syntaxKind)
        {

        }

        public override Token Clone() => new ConditionKeyword(Value, SyntaxKind);
    }
}
