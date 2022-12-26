using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Statements
{
    internal sealed class AssignmentNode : StatementNode
    {
        public override NodeType NodeType => NodeType.Assignment;

        public Node Value { get => Children[0]; }
        public Node To { get => Children[1]; }

        public AssignmentNode(Node _value, Node _to)
        {
            Children = new List<Node>() { _value, _to };
        }

        public override string ToString() => $"Assign Node";
    }
}
