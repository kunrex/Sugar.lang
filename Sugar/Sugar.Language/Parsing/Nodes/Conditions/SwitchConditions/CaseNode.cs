using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.CtrlStatements;

using Sugar.Language.Parsing.Nodes.Interfaces.Creation;

namespace Sugar.Language.Parsing.Nodes.Conditions.SwitchConditions
{
    internal sealed class CaseNode : ParseNodeCollection, ICreationNode_Body
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Case; }

        private readonly ParseNodeCollection expression;
        public ParseNodeCollection Expression { get => expression; }

        private readonly ParseNode body;
        public ParseNode Body { get => body; }

        private readonly ControlStatement control;
        public ControlStatement Control { get => control; }

        public bool IsFallThrough { get => body == null; }

        public CaseNode(ParseNodeCollection _expression) : base(_expression)
        {
            expression = _expression;

            body = null;
            control = null;
        }

        public CaseNode(ParseNodeCollection _expression, ParseNode _body, ControlStatement _control) : base(_expression, _body, _control)
        {
            expression = _expression;

            body = _body;
            control = _control;
        }

        public override ParseNode AddChild(ParseNode node) { return this; }

        public override string ToString() => $"Case Node";
    }
}
