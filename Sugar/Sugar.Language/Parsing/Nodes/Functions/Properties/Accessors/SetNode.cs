using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Functions.Properties.Accessors
{
    internal sealed class SetNode : AccessorNode
    {
        public override NodeType NodeType => NodeType.Set;

        public Node ValueNode { get => Children[1]; }
        public override Node Body { get => Children[2]; }

        public SetNode(Node _describer, Node _valueDeclaration, Node _body) : base(_describer, _body)
        {
            Children = new List<Node>() { _describer, _valueDeclaration, _body };
        }

        public override string ToString() => $"Set Node";
    }
}
