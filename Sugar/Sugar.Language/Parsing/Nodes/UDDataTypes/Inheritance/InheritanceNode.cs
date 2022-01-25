using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.UDDataTypes.Inheritance
{
    internal sealed class InheritanceNode : Node
    {
        public override NodeType NodeType => NodeType.Inheritance;

        public InheritanceNode(List<Node> _types)
        {
            Children = _types;
        }

        public override string ToString() => $"Inheritance Node";
    }
}
