using System;

namespace Sugar.Language.Parsing.Nodes.Expressions
{
    internal abstract class ExpressionNode : ParseNodeCollection
    {
        public ExpressionNode(params ParseNodeCollection[] _children) : base(_children)
        {

        }

        public override ParseNode AddChild(ParseNode _node) { return this; }
    }
}
