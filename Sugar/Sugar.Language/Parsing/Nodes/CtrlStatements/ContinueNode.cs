using System;
using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.CtrlStatements
{
    internal sealed class ContinueNode : ControlStatement
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Continue; }

        public ContinueNode()
        {

        }

        public override string ToString() => $"Continue Node";
    }
}
