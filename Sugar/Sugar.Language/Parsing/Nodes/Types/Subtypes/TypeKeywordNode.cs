using System;

using Sugar.Language.Tokens.Keywords;
using Sugar.Language.Parsing.Nodes.Types.Enums;

namespace Sugar.Language.Parsing.Nodes.Types.Subtypes
{
    internal sealed class TypeKeywordNode : TypeNode
    {
        public override TypeNodeEnum Type { get => TypeNodeEnum.BuiltIn; }

        private readonly Keyword keyword;
        public Keyword Keyword { get => keyword; }

        public TypeKeywordNode(Keyword _keyword)
        {
            keyword = _keyword;
        }

        public override string ToString() => $"Type Keyword Node [Type: {keyword.Value}]";
    }
}
