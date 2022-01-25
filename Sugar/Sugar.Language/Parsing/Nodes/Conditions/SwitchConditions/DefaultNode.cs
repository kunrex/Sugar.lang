using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Conditions.SwitchConditions
{
    internal sealed class DefaultNode : Node
    {
        public override NodeType NodeType => NodeType.Default; 

        private Node Body { get => Children[0]; }
        private Node ControlStatement { get => Children[1]; }

        public DefaultNode(Node body, Node controlStatement)
        {
            Children = new List<Node>() { body, controlStatement };
        }

        public override string ToString() => $"Default Node";
    }
}
