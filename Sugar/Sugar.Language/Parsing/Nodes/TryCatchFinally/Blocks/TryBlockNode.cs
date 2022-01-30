using System;
using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.TryCatchFinally.Blocks
{
    internal sealed class TryBlockNode : BlockNode
    {
        public override NodeType NodeType => NodeType.Try;

        public TryBlockNode(Node _body) : base(_body)
        {

        }

        public override string ToString() => $"Try Node";
    }
}
