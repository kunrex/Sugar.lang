using System;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Loops
{
    internal sealed class ForLoopNode : LoopNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.For; }

        private readonly ParseNode initialise;
        public ParseNode Initialise { get => initialise; }

        private readonly ParseNode increment;
        public ParseNode Increment { get => increment; }

        public ForLoopNode(ParseNode _initialise, ParseNodeCollection _condition, ParseNode _increment, ParseNode _body) : base(_condition, _body)
        {
            initialise = _initialise;
            Add(initialise);

            increment = _increment;
            Add(initialise);
        }

        public override string ToString() => $"For Loop Node";
    }
}
