using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes
{
    internal sealed class SugarFileGroupNode : ParseNodeCollection
    {
        public override ParseNodeType NodeType { get => ParseNodeType.SugarFile; }

        public SugarFileGroupNode() : base()
        {

        }

        public SugarFileGroupNode(List<ParseNode> _children) : base(_children)
        {

        }

        public override string ToString() => $"Sugar File Node";
    }
}
