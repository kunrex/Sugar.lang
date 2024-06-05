using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.NodeGroups
{
    internal sealed class ScopeNode : CompoundStatementNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Scope; }

        public ScopeNode(List<ParseNode> _children) : base(_children)
        {

        }

        public override string ToString() => $"Scope Node";
    }
}
