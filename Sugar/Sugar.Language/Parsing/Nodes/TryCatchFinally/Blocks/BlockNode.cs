using System;

using Sugar.Language.Parsing.Nodes.Interfaces.Creation;

namespace Sugar.Language.Parsing.Nodes.TryCatchFinally.Blocks
{
    internal abstract class BlockNode : ParseNodeCollection, ICreationNode_Body
    {
        private readonly ParseNode body;
        public ParseNode Body { get => body; }

        public BlockNode(ParseNode _body) : base(_body)
        {
            body = _body;
        }

        public override ParseNode AddChild(ParseNode node) { return this; }
    }
}
