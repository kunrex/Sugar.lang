using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Conditions.SwitchConditions
{
    internal sealed class WhenNode : Node
    {
        public override NodeType NodeType => NodeType.Switch;

        public Node Declaration { get => Children[0]; }
        public Node Expression { get => Children[1]; }

        public WhenNode(Node _declaration, Node _expresssion)
        {
            Children = new List<Node>() { _declaration, _expresssion };
        }

        public override string ToString() => $"When Node";
    }
}
