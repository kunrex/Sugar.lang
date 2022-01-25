using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Functions.Calling.Structure
{
    internal sealed class FunctionNameCallNode : Node
    {
        public Node Value { get => Children[0]; }
        public override NodeType NodeType => NodeType.FunctionCall;

        public FunctionNameCallNode(Node _value)
        {
            Children = new List<Node>() { _value };
        }

        public override string ToString() => $"Function Name Node";
    }
}
