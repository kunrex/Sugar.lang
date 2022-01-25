using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Conditions.SwitchConditions
{
    internal sealed class SwitchNode : Node
    {
        public override NodeType NodeType => NodeType.Switch;
        private Node Value { get => Children[0]; }

        public SwitchNode(Node _valueToCheck, List<Node> _cases)
        {
            Children = new List<Node>() { _valueToCheck };
            Children.AddRange(_cases);
        }

        public override string ToString() => $"Switch Node";
    }
}
