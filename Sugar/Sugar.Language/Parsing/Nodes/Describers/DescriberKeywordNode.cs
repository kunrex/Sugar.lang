using System;

using Sugar.Language.Tokens.Keywords;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Describers
{
    internal class DescriberKeywordNode : ParseNodeCollection
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Describer; }

        protected readonly Keyword keyword;
        public Keyword Keyword { get => keyword; }

        public string Name { get => keyword.Value; }

        public DescriberKeywordNode(Keyword _keyword)
        {
            keyword = _keyword;
        }

        public override string ToString() => $"Describer Keyword Node [Value: {keyword.Value}]";
    }
}
