using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.NodeGroups
{
    internal class ExpressionListNode : Node
    {
        public override NodeType NodeType => NodeType.ExpressionList;

        public ExpressionListNode() : base()
        {

        }

        public ExpressionListNode(List<Node> _children) : base(_children)
        {

        }

        public ExpressionListNode(Node _children) : base(new List<Node> { _children })
        {

        }

        public override string ToString() => $"Expression List Node";
    }
}
