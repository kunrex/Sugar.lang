using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.CtrlStatements;

using Sugar.Language.Parsing.Nodes.Interfaces.Creation;

namespace Sugar.Language.Parsing.Nodes.Conditions.SwitchConditions
{
    internal sealed class DefaultNode : ParseNodeCollection, ICreationNode_Body
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Default; }

        private readonly ParseNode body;
        public ParseNode Body { get => body; }

        private readonly ControlStatement control;
        public ControlStatement Control { get => control; }

        public DefaultNode(ParseNode _body, ControlStatement _control) : base(_body, _control)
        {
            body = _body;
            control = _control;
        }

        public override ParseNode AddChild(ParseNode node) { return this; }

        public override string ToString() => $"Default Node";
    }
}
