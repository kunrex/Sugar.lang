using System;
using System.Collections.Generic;

namespace Sugar.Language.Parsing.Nodes
{
    internal abstract class CollectionNode : Node
    {
        public CollectionNode()
        {
            Children = new List<Node>();
        }

        public CollectionNode(List<Node> children)
        {
            Children = children;
        }
    }
}
