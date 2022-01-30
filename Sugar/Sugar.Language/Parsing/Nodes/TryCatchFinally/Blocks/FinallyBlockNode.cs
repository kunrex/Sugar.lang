using System;
using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.TryCatchFinally.Blocks
{
    internal sealed class FinallyBlockNode : BlockNode
    {
        public override NodeType NodeType => NodeType.Finally;

        public FinallyBlockNode(Node _body) : base(_body)
        {

        }

        public override string ToString() => $"Finally Node";
    }
}
