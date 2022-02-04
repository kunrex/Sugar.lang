using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Values
{
    internal sealed class DefaultTypeNode : Node
    {
        public override NodeType NodeType => NodeType.Default;

        public Node Type { get => Children[0]; }

        public DefaultTypeNode(Node _type)
        {
            Children = new List<Node>() { _type };
        }

        public override string ToString() => $"Default Type Node";
    }
}
