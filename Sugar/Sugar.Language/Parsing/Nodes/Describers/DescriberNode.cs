using System;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Describers
{
    internal class DescriberNode : NodeCollection<DescriberKeywordNode>
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Describer; }

        public DescriberNode() : base()
        {

        }

        public DescriberNode AddChild(DescriberKeywordNode node)
        {
            Add(node);

            return this;
        }

        public override string ToString() => $"Describer Node";
    }
}
