using System;

namespace Sugar.Language.Parsing.Nodes.CtrlStatements
{
    internal abstract class ControlStatement : ParseNodeCollection
    {
        public ControlStatement()
        {

        }

        public ControlStatement(ParseNode _child) : base(_child)
        {

        }

        public override ParseNode AddChild(ParseNode node) { return this; }
    }
}
