using System;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Loops
{
    internal sealed class DoWhileNode : LoopNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.DoWhile; }

        public DoWhileNode(ParseNodeCollection _condition, ParseNode _body) : base(_condition, _body)
        {
           
        }

        public override string ToString() => $"Do While Loop Node";
    }
}
