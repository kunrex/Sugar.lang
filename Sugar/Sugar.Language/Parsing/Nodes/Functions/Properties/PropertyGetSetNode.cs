using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Functions.Properties
{
    internal sealed class PropertyGetSetNode : PropertyNode
    {
        public Node Get { get => Children[2]; }
        public Node Set { get => Children[3]; }

        public override NodeType NodeType => NodeType.PropertyGetSet;

        public PropertyGetSetNode(Node _name, Node _type, Node _get, Node _set) : base(_name, _type)
        {
            Children = new List<Node>() { _name, _type, _get, _set };
        }

        public override string ToString() => $"Get Set Property Node";
    }
}
