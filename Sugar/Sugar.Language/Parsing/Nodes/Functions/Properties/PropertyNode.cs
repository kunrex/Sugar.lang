using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Interfaces.Creation;

namespace Sugar.Language.Parsing.Nodes.Functions.Properties
{
    internal abstract class PropertyNode : Node, ICreationNode_Name, ICreationNode_Type
    {
        public Node Name { get => Children[0]; }
        public Node Type { get => Children[1]; }

        public Node Describer { get => null; }

        public PropertyNode(Node _name, Node _type) 
        {
            Children = new List<Node>() { _name, _type };
        }
    }
}
