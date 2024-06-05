using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Values.Generics;

using Sugar.Language.Parsing.Nodes.Functions.Calling.Structure;

namespace Sugar.Language.Parsing.Nodes.Functions.Calling
{
    internal sealed class FunctionCallNode : BaseFunctionCallNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.FunctionCall; }

        public FunctionCallNode(ParseNodeCollection _value, FunctionArgumentsNode _arguments) : base(_value, _arguments)
        {
            
        }

        public FunctionCallNode(ParseNodeCollection _value, FunctionArgumentsNode _arguments, GenericCallNode _generic) : base(_value, _arguments, _generic)
        {

        }

        public override string ToString() => $"Function Call Node";
    }
}
