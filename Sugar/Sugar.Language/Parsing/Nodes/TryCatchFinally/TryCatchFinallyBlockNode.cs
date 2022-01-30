using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.TryCatchFinally
{
    internal sealed class TryCatchFinallyBlockNode : Node
    {
        public override NodeType NodeType => NodeType.TryCatchFinally;

        public Node Try { get => Children[0]; }
        public Node Catch { get => Children[0]; }
        public Node Finally { get => Children[0]; }

        public TryCatchFinallyBlockNode(Node _try, Node _catch, Node _finally)
        {
            Children = new List<Node>() { _try, _catch, _finally };
        }

        public override string ToString() => $"Try Catch Finally Node";
    }
}
