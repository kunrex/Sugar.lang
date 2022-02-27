using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.NodeGroups
{
    internal sealed class ExpressionListNode : Node
    {
        public override NodeType NodeType => NodeType.ExpressionList;

        public ExpressionListNode() : base()
        {

        }

        public ExpressionListNode(List<Node> _children) : base(_children)
        {

        }

        public override string ToString() => $"Expression List Node";
    }
}
