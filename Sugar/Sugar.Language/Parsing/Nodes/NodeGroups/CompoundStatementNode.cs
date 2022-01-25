using System;
using System.Collections.Generic;

namespace Sugar.Language.Parsing.Nodes.NodeGroups
{
    internal sealed class CompoundStatementNode : NodeGroup
    {
        public CompoundStatementNode() : base()
        {

        }

        public CompoundStatementNode(List<Node> _children) : base(_children)
        {

        }

        public override string ToString() => "Compund Statement Node";
    }
}
