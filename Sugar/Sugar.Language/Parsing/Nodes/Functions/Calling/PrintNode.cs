using System;

using Sugar.Language.Tokens.Keywords;
using Sugar.Language.Parsing.Nodes.Types.Subtypes;
using Sugar.Language.Parsing.Nodes.Functions.Calling.Structure;

namespace Sugar.Language.Parsing.Nodes.Functions.Calling
{
    internal sealed class PrintNode : BaseFunctionCallNode
    {
        public PrintNode(FunctionCallArgumentsNode _value) : base(new FunctionNameCallNode(new TypeKeywordNode(Keyword.Print)), _value)
        {

        }

        public override string ToString() => $"Print Node";
    }
}
