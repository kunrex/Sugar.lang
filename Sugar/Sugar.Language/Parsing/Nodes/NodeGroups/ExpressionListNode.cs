using System;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.NodeGroups
{
    internal class ExpressionListNode : ParseNodeCollection
    {
        public override ParseNodeType NodeType { get => ParseNodeType.ExpressionList; }

        public ExpressionListNode() : base()
        {

        }

        public override string ToString() => $"Expression List Node";
    }
}
