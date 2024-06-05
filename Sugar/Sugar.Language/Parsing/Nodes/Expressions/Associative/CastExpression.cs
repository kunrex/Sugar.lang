using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Types;

namespace Sugar.Language.Parsing.Nodes.Expressions.Associative
{
    internal sealed class CastExpression : BaseBinaryNode<ParseNodeCollection, TypeNode>
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Cast; }

        public CastExpression(ParseNodeCollection _operhand, TypeNode _type) : base(_operhand, _type)
        {
          
        }

        public override string ToString() => $"Cast Node";
    }
}
