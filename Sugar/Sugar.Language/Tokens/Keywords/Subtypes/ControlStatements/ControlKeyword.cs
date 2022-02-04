using System;
using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Keywords.Subtypes.ControlStatements
{
    internal sealed partial class ControlKeyword : Keyword
    {
        public override KeywordType KeywordType { get => KeywordType.ControlStatement; }

        private ControlKeyword(string _value, SyntaxKind _syntaxKind) : base(_value, _syntaxKind)
        {

        }

        public override Token Clone() => new ControlKeyword(Value, SyntaxKind);
    }
}

