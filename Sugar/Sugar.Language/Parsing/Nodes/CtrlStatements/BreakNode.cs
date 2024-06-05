using System;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.CtrlStatements
{
    internal sealed class BreakNode : ControlStatement
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Break; }

        public BreakNode()
        {

        }

        public override string ToString() => $"Break Node";
    }
}
