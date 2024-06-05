using System;

using Sugar.Language.Parsing.Nodes.NodeGroups;

namespace Sugar.Language.Parsing.Nodes.Statements
{
    internal abstract class StatementNode : CompoundStatementNode
    {
        public StatementNode(params ParseNode[] _children) : base(_children)
        {
            
        }

        public override ParseNode AddChild(ParseNode _node) { return this; }
    }
}
