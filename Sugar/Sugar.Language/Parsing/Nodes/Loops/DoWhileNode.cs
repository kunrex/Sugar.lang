using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Loops
{
    internal sealed class DoWhileNode : LoopNode
    {
        public override NodeType NodeType => NodeType.DoWhile;

        public override Node Body { get => Children[0]; }
        public override Node Condition { get => Children[1]; }

        public DoWhileNode(Node _condition, Node _body)
        {
            Children = new List<Node>() { _body, _condition };
        }

        public override string ToString() => $"Do While Loop Node";
    }
}
