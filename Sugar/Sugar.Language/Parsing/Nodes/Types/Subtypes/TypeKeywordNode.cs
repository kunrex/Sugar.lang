using System;

using Sugar.Language.Tokens.Keywords;
using Sugar.Language.Parsing.Nodes.Types.Enums;

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
    }
}
