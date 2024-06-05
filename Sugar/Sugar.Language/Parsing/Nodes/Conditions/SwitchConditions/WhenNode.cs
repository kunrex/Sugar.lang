using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Statements.VariableCreation;

namespace Sugar.Language.Parsing.Nodes.Conditions.SwitchConditions
{
    internal sealed class WhenNode : ParseNodeCollection
    {
        public override ParseNodeType NodeType { get => ParseNodeType.When; }

        private readonly DeclarationNode declaration;
        public DeclarationNode Declaration { get => declaration; }

        private readonly ParseNodeCollection expression;
        public ParseNodeCollection Expression { get => expression; }

        public WhenNode(DeclarationNode _declaration, ParseNodeCollection _expresssion) : base(_declaration, _expresssion)
        {
            declaration = _declaration;
            expression = _expresssion;
        }

        public override ParseNode AddChild(ParseNode node) { return this; }

        public override string ToString() => $"When Node";
    }
}
