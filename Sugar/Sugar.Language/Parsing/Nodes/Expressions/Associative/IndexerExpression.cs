using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.NodeGroups;

namespace Sugar.Language.Parsing.Nodes.Expressions.Associative
{
    internal class IndexerExpression : BaseBinaryNode<ParseNodeCollection, ExpressionListNode>
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Indexer; }

        public IndexerExpression(ParseNodeCollection _operhand, ExpressionListNode _arguments) : base(_operhand, _arguments)
        {
           
        }
        public override string ToString() => $"Indexer Expression";
    }
}
