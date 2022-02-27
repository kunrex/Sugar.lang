using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.NodeGroups
{
    internal sealed class ScopeNode : Node
    {
        public override NodeType NodeType => NodeType.Scope;

        public ScopeNode() : base()
        {

        }

        public ScopeNode(List<Node> _children) : base(_children)
        {

        }

        public override string ToString() => $"Scope Node";
    }
}
