using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Keywords.Subtypes.Describers
{
    internal sealed partial class DescriberKeyword : Keyword
    {
        public DescriberType DescriberType { get; private set; }
        public override KeywordType KeywordType { get => KeywordType.Describer; }

        public DescriberKeyword(string _value, SyntaxKind _syntaxKind, DescriberType _describerType) : base(_value, _syntaxKind)
        {
            DescriberType = _describerType;
        }

        public override Token Clone() => new DescriberKeyword(Value, SyntaxKind, DescriberType);
    }
}
