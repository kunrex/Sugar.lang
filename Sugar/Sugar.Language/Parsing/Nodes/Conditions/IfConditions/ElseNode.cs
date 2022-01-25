using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Conditions.IfConditions
{
    internal sealed class ElseNode : Node
    {
        public override NodeType NodeType => NodeType.Else;
        private Node Body { get => Children[0]; }

        public ElseNode(Node _body)
        {
            Children = new List<Node>() { _body };
        }

        public override string ToString() => $"Else Node";
    }
}
