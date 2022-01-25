using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.NodeGroups;

namespace Sugar.Language.Parsing.Nodes.Expressions.Associative
{
    internal sealed class IndexerArgumentsNode : NodeGroup
    {
        public IndexerArgumentsNode(List<Node> _children) : base(_children)
        {
        }

        public override string ToString() => $"Indexer Arguments Node";
    }
}
