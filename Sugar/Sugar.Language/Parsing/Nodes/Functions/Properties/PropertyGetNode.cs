using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Functions.Properties
{
    internal sealed class PropertyGetNode : PropertyNode
    {
        public Node Get { get => Children[1]; }
        public override NodeType NodeType => NodeType.PropertyGet;

        public PropertyGetNode(Node _name, Node _get) : base(_name)
        {
            Children = new List<Node>() { _name, _get };
        }

        public override string ToString() => $"Only Get Property Node";
    }
}
