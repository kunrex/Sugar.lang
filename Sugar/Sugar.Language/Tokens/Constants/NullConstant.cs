using System;

using Sugar.Language.Tokens.Enums;
using Sugar.Language.Semantics.Analysis.BuiltInTypes.Enums;

namespace Sugar.Language.Tokens.Constants
{
    internal sealed class NullConstant : Constant
    {
        public static readonly NullConstant Null = new NullConstant("null");

        public override ConstantType ConstantType { get => ConstantType.Null; }

        private NullConstant(string _value) : base(_value)
        {
            SyntaxKind = SyntaxKind.Null; 
        }

        public override Token Clone() => new NullConstant(Value);
    }
}
