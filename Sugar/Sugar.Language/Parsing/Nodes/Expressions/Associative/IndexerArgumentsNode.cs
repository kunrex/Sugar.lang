using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Expressions.Associative
{
    internal sealed class IndexerArgumentsNode : Node
    {
        public override NodeType NodeType { get => NodeType.ExpressionList; }

        public IndexerArgumentsNode(List<Node> _children) : base(_children)
        {

        }

        public override string ToString() => $"Indexer Arguments Node";
    }
}
