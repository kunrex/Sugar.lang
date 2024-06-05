using System;

using Sugar.Language.Tokens.Keywords;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Values
{
    internal sealed class BuiltInFunctionNode : ValueNode<Keyword>
    {
        public override ParseNodeType NodeType { get => ParseNodeType.BuiltInFunction; }

        public BuiltInFunctionNode(Keyword _keyword) : base(_keyword)
        {

        }

        public override string ToString() => $"Built In Function Node [Value: {Value}]";
    }
}
