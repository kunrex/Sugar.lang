using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Semantics.Analysis;

namespace Sugar.Language.Parsing.Nodes.NodeGroups
{
    internal abstract class NodeGroup : Node
    {
        public override NodeType NodeType => NodeType.Group;

        public NodeGroup() : base()
        {

        }

        public NodeGroup(List<Node> _children) : base(_children)
        {

        }
    }
}
