using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Interfaces.Creation;

namespace Sugar.Language.Parsing.Nodes.Functions.Properties.Accessors
{
    internal abstract class AccessorNode : Node, ICreationNode
    {
        public Node Describer { get => Children[0]; }
        public virtual Node Body { get => Children[1]; }

        public AccessorNode(Node _describer, Node _body)
        {
            Children = new List<Node>() { _describer, _body };
        }
    }
}
