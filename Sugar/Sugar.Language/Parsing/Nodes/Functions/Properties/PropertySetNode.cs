using System;
using System.Collections.Generic;

namespace Sugar.Language.Parsing.Nodes.Functions.Properties
{
    internal sealed class PropertySetNode : PropertyNode
    {
        public Node Set { get => Children[1]; }

        public PropertySetNode(Node _name, Node _set) : base(_name)
        {
            Children = new List<Node>() { _name, _set };
        }
        public override string ToString() => $"Only Set property Node";
    }
}
