using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Functions.Properties
{
    internal sealed class PropertySetNode : PropertyNode
    {
        public Node Set { get => Children[2]; }

        public override NodeType NodeType => NodeType.PropertySet;

        public PropertySetNode(Node _name, Node _type, Node _set) : base(_name, _type)
        {
            Children = new List<Node>() { _name, _type, _set };
        }
        public override string ToString() => $"Only Set property Node";
    }
}
