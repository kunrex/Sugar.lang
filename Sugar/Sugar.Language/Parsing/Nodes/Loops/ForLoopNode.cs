using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Loops
{
    internal sealed class ForLoopNode : LoopNode
    {
        public override NodeType NodeType => NodeType.For;

        public Node Initialise { get => Children[0]; }
        public override Node Condition { get => Children[1]; }
        public Node Increment { get => Children[2]; }

        public override Node Body { get => Children[3]; }

        public ForLoopNode(Node _initialise, Node _condition, Node _increment, Node _body)
        {
            Children = new List<Node>() { _initialise, _condition, _increment, _body };
        }

        public override string ToString() => $"For Loop Node";
    }
}
