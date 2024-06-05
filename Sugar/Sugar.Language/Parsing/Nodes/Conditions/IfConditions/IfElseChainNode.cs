using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.NodeGroups;

namespace Sugar.Language.Parsing.Nodes.Conditions.IfConditions
{
    internal sealed class IfElseChainNode : CompoundStatementNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.IfElseChain; }

        public IfElseChainNode(List<ParseNodeCollection> _conditions) : base()
        {
            foreach (var condition in _conditions)
                Add(condition);
        }

        public override string ToString() => $"If Else Chain";
    }
}
