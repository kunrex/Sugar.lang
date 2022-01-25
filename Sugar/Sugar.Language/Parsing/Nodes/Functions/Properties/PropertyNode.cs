using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Functions.Properties
{
    internal abstract class PropertyNode : Node
    {
        public Node Name { get => Children[0]; }
        public override NodeType NodeType => NodeType.Property;

        public PropertyNode(Node _name) 
        {
            Children = new List<Node>() { _name };
        }
    }
}
