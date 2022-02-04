using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Values
{
    internal sealed class AsTypeNode : Node
    {
        public override NodeType NodeType { get => NodeType.AsType; }

        public Node Type { get => Children[0]; }

        public AsTypeNode(Node _type)
        {
            Children = new List<Node>() { _type };
        }

        public override string ToString() => $"As Type Node";
    }
}
