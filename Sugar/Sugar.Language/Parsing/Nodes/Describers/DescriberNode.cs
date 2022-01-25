using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Describers
{
    internal sealed class DescriberNode : Node
    {
        public override NodeType NodeType => NodeType.Describer;

        public DescriberNode()
        {

        }

        public DescriberNode(Node _keyword)
        {
            Children = new List<Node>() { _keyword };
        }

        public DescriberNode(List<Node> _keywords)
        {
            Children = _keywords;
        }

        public override string ToString() => $"Describer Node";
    }
}
