using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Values
{
    internal sealed class ParentNode : Node
    {
        public override NodeType NodeType => NodeType.Parent;

        public Node Reference { get => Children[0]; }

        public ParentNode(Node _reference)
        {
            Children = new List<Node>() { _reference };
        }

        public override string ToString() => $"This Node";
    }
}
