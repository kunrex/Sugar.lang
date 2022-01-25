using System;

using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Functions.Calling.Structure;

namespace Sugar.Language.Parsing.Nodes.Functions.Calling
{
    internal sealed class ConstructorCallNode : BaseFunctionCallNode
    {
        public override NodeType NodeType => NodeType.ConstructorCall;

        public ConstructorCallNode(FunctionNameCallNode _value, FunctionCallArgumentsNode _arguments) : base(_value, _arguments)
        {

        }

        public override string ToString() => $"Constructor Call Node";
    }
}
