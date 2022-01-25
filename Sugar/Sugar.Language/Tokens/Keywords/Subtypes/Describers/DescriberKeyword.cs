using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Keywords.Subtypes.Describers
{
    internal sealed partial class DescriberKeyword : Keyword
    {
        public DescriberType DescriberType { get; private set; }
        public override KeywordType KeywordType => KeywordType.Describer;

        public DescriberKeyword(string _value, byte _id, DescriberType _describerType) : base(_value, _id)
        {
            DescriberType = _describerType;
        }

        private DescriberKeyword() : base()
        {

        }

        public override Token Clone() => new DescriberKeyword()
        {
            Value = Value,
            DescriberType = DescriberType,
            Id = Id
        };
    }
}
