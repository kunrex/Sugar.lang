using System;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Values
{
    internal sealed class ThisNode : Node
    {
        public override NodeType NodeType => NodeType.This;

        public ThisNode()
        {

        }

        public override string ToString() => $"This Node";
    }
}
