using System;

using Sugar.Language.Tokens.Enums;

using Sugar.Language.Tokens.Keywords;
using Sugar.Language.Parsing.Nodes.Types.Enums;

using Sugar.Language.Semantics.Analysis.BuiltInTypes.Enums;

namespace Sugar.Language.Parsing.Nodes.Types.Subtypes
{
    internal sealed class TypeKeywordNode : TypeNode
    {
        private readonly Keyword keyword;
        public Keyword Keyword { get => keyword; }
        public override TypeNodeEnum Type => TypeNodeEnum.BuiltIn;

        public TypeKeywordNode(Keyword _type)
        {
            keyword = _type;
        }

        public override string ToString() => $"Type Keyword Node [Type: {keyword.Value}]";

        public override TypeEnum ReturnType() => keyword.SyntaxKind switch
        {
            SyntaxKind.Byte => TypeEnum.Byte,
            SyntaxKind.SByte => TypeEnum.SByte,

            SyntaxKind.Short => TypeEnum.Short,
            SyntaxKind.UShort => TypeEnum.UShort,

            SyntaxKind.Int => TypeEnum.Int,
            SyntaxKind.UInt => TypeEnum.UInt,

            SyntaxKind.Long => TypeEnum.Long,
            SyntaxKind.Ulong => TypeEnum.ULong,

            SyntaxKind.Float => TypeEnum.Float,
            SyntaxKind.Double => TypeEnum.Double,
            SyntaxKind.Decimal => TypeEnum.Decimal,

            SyntaxKind.Char => TypeEnum.Char,
            SyntaxKind.String => TypeEnum.String,

            SyntaxKind.Bool => TypeEnum.Boolean,

            _ => throw new InvalidOperationException()
        };
    }
}
