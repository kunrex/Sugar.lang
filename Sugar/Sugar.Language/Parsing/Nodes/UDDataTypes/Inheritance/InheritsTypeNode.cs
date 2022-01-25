using System;
using System.Collections.Generic;
using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.UDDataTypes.Inheritance
{
    internal sealed class InheritsTypeNode : Node
    {
        public override NodeType NodeType => NodeType.Inheritance;

        public InheritsTypeNode(Node _type)
        {
            Children = new List<Node>() { _type };
        }

        public override string ToString() => $"Inherits Type Node";
    }
}
