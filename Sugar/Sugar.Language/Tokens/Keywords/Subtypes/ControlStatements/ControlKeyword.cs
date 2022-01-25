using System;
using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Keywords.Subtypes.ControlStatements
{
    internal sealed partial class ControlKeyword : Keyword
    {
        public override KeywordType KeywordType => KeywordType.ControlStatement;

        private ControlKeyword(string _value, byte _id) : base(_value, _id)
        {

        }

        private ControlKeyword() : base()
        {

        }

        public override Token Clone() => new ControlKeyword()
        {
            Value = Value,
            Id = Id
        };
    }
}

