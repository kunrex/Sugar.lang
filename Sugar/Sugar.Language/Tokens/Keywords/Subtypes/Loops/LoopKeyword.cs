using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Keywords.Subtypes.Loops
{
    internal sealed partial class LoopKeyword : Keyword
    {
        public override KeywordType KeywordType { get => KeywordType.Loop; }

        private LoopKeyword(string _value, SyntaxKind _syntaxKind) : base(_value, _syntaxKind)
        {

        }

        public override Token Clone() => new LoopKeyword(Value, SyntaxKind);
    }
}
