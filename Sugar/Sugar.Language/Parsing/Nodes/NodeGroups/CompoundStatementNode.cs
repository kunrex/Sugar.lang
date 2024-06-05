using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.NodeGroups
{
    internal class CompoundStatementNode : ParseNodeCollection
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Compound; }

        public CompoundStatementNode() : base()
        {

        }

        public CompoundStatementNode(List<ParseNode> _children) : base(_children)
        {

        }

        public CompoundStatementNode(params ParseNode[] _children) : base(_children)
        {

        }

        public override ParseNode AddChild(ParseNode _node) { return this; }

        public override string ToString() => "Compund Statement Node";
    }
}
