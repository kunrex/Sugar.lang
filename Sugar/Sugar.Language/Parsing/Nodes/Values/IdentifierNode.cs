using System;

using Sugar.Language.Tokens;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Values
{
    internal class IdentifierNode : ValueNode<Identifier>
    {
        public static IdentifierNode ValueIdentifier = new IdentifierNode(new Identifier("value", -1));

        public override ParseNodeType NodeType { get => ParseNodeType.Identifier; }

        public IdentifierNode(Identifier _variable) : base(_variable)
        {
            
        }

        public override string ToString() => $"Identifier Node [Value: {Value}]";
    }
}
