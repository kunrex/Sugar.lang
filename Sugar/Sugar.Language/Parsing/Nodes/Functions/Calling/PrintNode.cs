using System;

using Sugar.Language.Tokens.Keywords;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Parsing.Nodes.Functions.Calling.Structure;

namespace Sugar.Language.Parsing.Nodes.Functions.Calling
{
    internal sealed class PrintNode : BaseFunctionCallNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Print; }

        public PrintNode(Keyword _keyword, FunctionArgumentsNode _value) : base(new BuiltInFunctionNode(_keyword), _value)
        {

        }

        public override string ToString() => $"Print Node";
    }
}
