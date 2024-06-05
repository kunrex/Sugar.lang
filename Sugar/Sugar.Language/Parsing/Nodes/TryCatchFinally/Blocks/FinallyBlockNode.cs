using System;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.TryCatchFinally.Blocks
{
    internal sealed class FinallyBlockNode : BlockNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Finally; }

        public FinallyBlockNode(ParseNode _body) : base(_body)
        {
           
        }

        public override string ToString() => $"Finally Node";
    }
}
