using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.NodeGroups
{
    internal sealed class SugarFileGroupNode : Node
    {
        public override NodeType NodeType => NodeType.SugarFile;

        public SugarFileGroupNode() : base()
        {

        }

        public SugarFileGroupNode(List<Node> _children) : base(_children)
        {

        }

        public override string ToString() => $"Sugar File Node";
    }
}
