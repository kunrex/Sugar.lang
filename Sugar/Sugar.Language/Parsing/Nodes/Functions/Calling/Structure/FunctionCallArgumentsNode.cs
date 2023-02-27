using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.NodeGroups;

namespace Sugar.Language.Parsing.Nodes.Functions.Calling.Structure
{
    internal sealed class FunctionCallArgumentsNode : ExpressionListNode
    {
        public override NodeType NodeType => NodeType.ArgumentCall;

        public FunctionCallArgumentsNode()
        {
            
        }

        public FunctionCallArgumentsNode(Node _argument) : base(_argument)
        {
            
        }

        public FunctionCallArgumentsNode(List<Node> _arguments) : base(_arguments)
        {
            
        }

        public override string ToString() => $"Function Call Arguments Node";
    }
}
