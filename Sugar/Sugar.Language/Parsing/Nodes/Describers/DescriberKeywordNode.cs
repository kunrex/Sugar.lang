using System;

using Sugar.Language.Tokens.Keywords;
using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Describers
{
    internal sealed class DescriberKeywordNode : Node
    {
        private readonly Keyword keyword;
        public Keyword Keyword { get => keyword; }

        public override NodeType NodeType => NodeType.Describer;

        public DescriberKeywordNode(Keyword _keyword)
        {
            keyword = _keyword;
        }

        public override string ToString() => $"Describer Keyword Node [Value: {keyword.Value}]";
    }
}
