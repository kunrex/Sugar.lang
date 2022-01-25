using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Functions.Calling.Structure
{
    internal sealed class FunctionCallArgumentsNode : Node
    {
        public override NodeType NodeType => NodeType.ArgumentCall;

        public FunctionCallArgumentsNode()
        {
            
        }

        public FunctionCallArgumentsNode(Node _argument)
        {
            Children = new List<Node>() { _argument };
        }

        public FunctionCallArgumentsNode(List<Node> _arguments)
        {
            Children = _arguments;
        }

        public override string ToString() => $"Function Call Arguments Node";
    }
}
