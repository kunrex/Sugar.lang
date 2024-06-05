using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Interfaces.Creation;

namespace Sugar.Language.Parsing.Nodes.Conditions.IfConditions
{
    internal sealed class IfNode : ParseNodeCollection, ICreationNode_Body
    {
        public override ParseNodeType NodeType { get => ParseNodeType.If; }

        private readonly ParseNodeCollection condition;
        public ParseNodeCollection Condition { get => condition; }

        private readonly ParseNode body;
        public ParseNode Body { get => body; }

        public IfNode(ParseNodeCollection _condition, ParseNode _body) : base(_condition, _body)
        {
            condition = _condition;
            body = _body;
        }

        public override string ToString() => "If Node";
    }
}
