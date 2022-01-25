using System;

using Sugar.Language.Tokens;
using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Values
{
    internal sealed class IdentifierNode : ValueNode<Identifier>
    {
        public override NodeType NodeType => NodeType.Variable;

        public IdentifierNode(Identifier _variable) : base(_variable)
        {
          
        }

        public override string ToString() => $"Variable Node [Value: {Value}]";
    }
}
