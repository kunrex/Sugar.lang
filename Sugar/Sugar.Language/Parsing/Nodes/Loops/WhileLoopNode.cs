using System;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Loops
{
    internal sealed class WhileLoopNode : LoopNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.While; }

        public WhileLoopNode(ParseNodeCollection _condition, ParseNode _body) : base(_condition, _body)
        {

        }

        public override string ToString() => $"While Loop Node";
    }
}
