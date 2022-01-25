using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Conditions.IfConditions
{
    internal sealed class IfElseChainNode : Node
    {
        public override NodeType NodeType => NodeType.IfElseChain;

        public IfElseChainNode(List<Node> _nodes)
        {
            Children = _nodes;
        }

        public override string ToString() => $"If Else Chain";
    }
}
