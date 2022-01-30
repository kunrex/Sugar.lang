using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.TryCatchFinally.Blocks
{
    internal sealed class CatchBlockNode : BlockNode
    {
        public override NodeType NodeType => NodeType.Catch;

        public Node Arguments { get => Children[0]; }
        public override Node Body { get => Children[1]; }

        public CatchBlockNode(Node _arguments, Node _body) : base(_body)
        {
            Children = new List<Node>() { _arguments, _body };
        }

        public override string ToString() => $"Catch Node";
    }
}


