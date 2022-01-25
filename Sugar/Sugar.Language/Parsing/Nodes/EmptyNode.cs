using System;
using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes
{
    internal sealed class EmptyNode : Node
    {
        public override NodeType NodeType => NodeType.Empty;
        public override string ToString() => $"Empty Node";
    }
}
