using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Interfaces.Creation;

namespace Sugar.Language.Parsing.Nodes.Conditions.IfConditions
{
    internal sealed class ElseNode : ParseNodeCollection, ICreationNode_Body
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Else; }

        private readonly ParseNode body;
        public ParseNode Body { get => body; }

        public ElseNode(ParseNode _body) : base(_body)
        {
            body = _body;
        }

        public override ParseNode AddChild(ParseNode node) { return this; }

        public override string ToString() => $"Else Node";
    }
}
