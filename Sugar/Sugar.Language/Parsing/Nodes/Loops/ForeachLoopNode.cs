using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Loops
{
    internal sealed class ForeachLoopNode : LoopNode
    {
        public override NodeType NodeType => NodeType.Foreach;

        public Node Declaration { get => Children[0]; }
        public Node Collection { get => Children[1]; }
        public override Node Body { get => Children[2]; }

        public override Node Condition { get => throw new NotImplementedException(); }

        public ForeachLoopNode(Node _declaration, Node _collection, Node _body)
        {
            Children = new List<Node>() { _declaration, _collection, _body };
        }

        public override string ToString() => "Foreach Loop Node";
    }
}
