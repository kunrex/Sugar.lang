using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Conditions.SwitchConditions
{
    internal sealed class CaseNode : Node
    {
        public override NodeType NodeType => NodeType.Get; 

        private Node Value { get => Children[0]; }
        private Node Body { get => Children[1]; }
        private Node ControlStatment { get => Children[2]; }

        private bool IsFallThrough { get => ChildCount == 1; }

        public CaseNode(Node value)
        {
            Children = new List<Node>() { value };
        }

        public CaseNode(Node value, Node body, Node controlStatement)
        {
            Children = new List<Node>() { value, body, controlStatement };
        }

        public override string ToString() => $"Case Node";
    }
}
