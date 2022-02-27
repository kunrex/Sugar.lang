using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.NodeGroups
{
    internal sealed class CompoundStatementNode : Node
    {
        public override NodeType NodeType => NodeType.Compound;

        public CompoundStatementNode() : base()
        {

        }

        public CompoundStatementNode(List<Node> _children) : base(_children)
        {

        }

        public override string ToString() => "Compund Statement Node";
    }
}
