using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Interfaces.Creation;

namespace Sugar.Language.Parsing.Nodes.UDDataTypes
{
    internal sealed class NamespaceNode : Node, ICreationNode_Name, ICreationNode_Body
    {
        public override NodeType NodeType => NodeType.Namespace;

        public Node Describer { get => Children[0]; }

        public Node Name { get => Children[1]; }
        public Node Body { get => Children[2]; }

        public NamespaceNode(Node _describer, Node _name, Node _body) 
        {
            Children = new List<Node>() { _describer, _name, _body };
        }

        public IEnumerable<Node> GetDataTypes()
        {
            if (Body.NodeType == NodeType.Group)
                foreach (var child in Body.GetChildren())
                    yield return child;
            else
                yield return Body;
        }

        public override string ToString() => $"Name Space Node";
    }
}
