using System;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.TryCatchFinally.Blocks
{
    internal sealed class TryBlockNode : BlockNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Try; }

        public TryBlockNode(ParseNode _body) : base(_body)
        {
            
        }

        public override string ToString() => $"Try Node";
    }
}
