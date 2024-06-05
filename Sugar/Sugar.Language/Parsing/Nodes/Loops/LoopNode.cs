using System;

using Sugar.Language.Parsing.Nodes.Interfaces.Creation;

namespace Sugar.Language.Parsing.Nodes.Loops
{
    internal abstract class LoopNode : ParseNodeCollection, ICreationNode_Body
    {
        protected readonly ParseNodeCollection condition;
        public ParseNodeCollection Condition { get => condition; }

        protected readonly ParseNode body;
        public ParseNode Body { get => body; }

        public LoopNode(ParseNode _body) : base(_body)
        {
            body = _body;
        }

        public LoopNode(ParseNodeCollection _condition, ParseNode _body) : base(_condition, _body)
        {
            condition = _condition;

            body = _body;
        }

        public override ParseNode AddChild(ParseNode _node) { return this; }
    }
}
