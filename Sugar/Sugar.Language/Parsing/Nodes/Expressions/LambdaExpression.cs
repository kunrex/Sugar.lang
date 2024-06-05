using System;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Expressions
{
    internal sealed class LambdaNode : BaseUnaryNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.LambdaExpression; }

        public LambdaNode(ParseNodeCollection _child) : base(_child)
        {
            
        }

        public override string ToString() => $"Lambda";
    }
}
