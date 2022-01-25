using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Loops
{
    internal sealed class WhileLoopNode : LoopNode
    {
        public override NodeType NodeType => NodeType.While;

        public override Node Body { get => Children[1]; }
        public override Node Condition { get => Children[0]; }

        public WhileLoopNode(Node _condition, Node _body)
        {
            Children = new List<Node>() { _condition, _body };
        }

        public override string ToString() => $"While Loop Node";
    }
}
