using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Conditions.IfConditions
{
    internal sealed class IfNode : Node
    {
        public override NodeType NodeType => NodeType.If;

        public Node Condition { get => Children[0] ; }
        public Node Body { get => Children[1] ; }

        public IfNode(Node _condition, Node _body)
        {
            Children = new List<Node>() { _condition, _body };
        }

        public override string ToString() => "If Node";
    }
}
