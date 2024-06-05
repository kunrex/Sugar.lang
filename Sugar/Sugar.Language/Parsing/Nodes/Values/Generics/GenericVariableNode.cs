using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.DataTypes;

namespace Sugar.Language.Parsing.Nodes.Values.Generics
{
    internal sealed class GenericVariableNode : ParseNodeCollection
    {
        public override ParseNodeType NodeType { get => ParseNodeType.GenericDeclarataion; }

        private readonly IdentifierNode variable;
        public IdentifierNode Variable { get => variable; }

        private readonly InheritanceNode enforcement;
        public InheritanceNode Enforcement { get => enforcement; }

        public GenericVariableNode(IdentifierNode _variable)  : base(_variable)
        {
            variable = _variable;
            enforcement = null;
        }

        public GenericVariableNode(IdentifierNode _variable, InheritanceNode _enforcement) : base(_variable, _enforcement)
        {
            variable = _variable;
            enforcement = _enforcement;
        }

        public override ParseNode AddChild(ParseNode node) { return this; }

        public override string ToString() => $"Generic Declaration Type Node";
    }
}
