using System;
using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Keywords.Subtypes.Functions
{
    internal sealed partial class FunctionKeyword : Keyword
    {
        public override KeywordType KeywordType { get => KeywordType.Function; }

        public FunctionKeyword(string _value, SyntaxKind _syntaxKind) : base(_value, _syntaxKind)
        {

        }

        public override Token Clone() => new FunctionKeyword(Value, SyntaxKind);
    }
}
