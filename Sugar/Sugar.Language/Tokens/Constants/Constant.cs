using System;

using Sugar.Language.Tokens.Enums;
using Sugar.Language.Semantics.Analysis.BuiltInTypes.Enums;

namespace Sugar.Language.Tokens.Constants
{
    internal abstract class Constant : Token
    {
        public abstract ConstantType ConstantType { get; }

        public override TokenType Type { get => TokenType.Constant; }

        public Constant(string _value) : base(_value, SyntaxKind.Constant)
        {
                
        }

        public override Token Clone() => throw new NotImplementedException();
    }
}
