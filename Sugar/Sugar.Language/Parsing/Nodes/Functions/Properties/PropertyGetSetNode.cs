using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Functions.Properties
{
    internal sealed class PropertyGetSetNode : PropertyNode
    {
        public Node Get { get => Children[1]; }
        public Node Set { get => Children[2]; }

        public override NodeType NodeType => NodeType.PropertyGetSet;

        public PropertyGetSetNode(Node _name, Node _get, Node _set) : base(_name)
        {
            Children = new List<Node>() { _name, _get, _set };
        }

        public override string ToString() => $"Get Set Property Node";
    }
}
