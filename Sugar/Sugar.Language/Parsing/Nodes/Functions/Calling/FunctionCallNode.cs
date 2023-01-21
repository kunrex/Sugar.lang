using System;

using Sugar.Language.Parsing.Nodes.Functions.Calling.Structure;

namespace Sugar.Language.Parsing.Nodes.Functions.Calling
{
    internal sealed class FunctionCallNode : BaseFunctionCallNode
    {
        public FunctionCallNode(FunctionNameCallNode _value, FunctionCallArgumentsNode _arguments) : base(_value, _arguments)
        {
            
        }

        public override string ToString() => $"Function Call Node";
    }
}
