using System;

using Sugar.Language.Tokens.Keywords;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Parsing.Nodes.Functions.Calling.Structure;

namespace Sugar.Language.Parsing.Nodes.Functions.Calling
{
    internal sealed class InputNode : BaseFunctionCallNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Input; }

        public InputNode(Keyword token, FunctionArgumentsNode _value) : base(new BuiltInFunctionNode(token), _value)
        {

        }

        public override string ToString() => $"Input Node";
    }
}
