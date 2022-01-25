using System;
using System.Collections.Generic;

namespace Sugar.Language.Parsing.Nodes.NodeGroups
{
    internal sealed class ScopeNode : NodeGroup
    {
        public ScopeNode() : base()
        {

        }

        public ScopeNode(List<Node> _children) : base(_children)
        {

        }

        public override string ToString() => $"Scope Node";
    }
}
