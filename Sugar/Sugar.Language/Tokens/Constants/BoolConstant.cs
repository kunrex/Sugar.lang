using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Constants
{
    internal sealed class BoolConstant : Constant
    {
        public static readonly BoolConstant True = new BoolConstant("true", SyntaxKind.True);
        public static readonly BoolConstant False = new BoolConstant("false", SyntaxKind.False);

        public override ConstantType ConstantType { get => ConstantType.Boolean; }

        private BoolConstant(string _value, SyntaxKind _syntaxKind) : base(_value)
        {
            Value = _value;
            SyntaxKind = _syntaxKind;
        }

        public override Token Clone() => new BoolConstant(Value, SyntaxKind);
    }
}
