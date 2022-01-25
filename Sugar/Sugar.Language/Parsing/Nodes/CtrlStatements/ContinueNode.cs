using System;
using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.CtrlStatements
{
    internal sealed class ContinueNode : ControlStatement
    {
        public override NodeType NodeType => NodeType.Continue;

        public ContinueNode()
        {

        }

        public override string ToString() => $"Continue Node";
    }
}
