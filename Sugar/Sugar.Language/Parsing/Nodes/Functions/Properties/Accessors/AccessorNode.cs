using System;

using Sugar.Language.Parsing.Nodes.Describers;

using Sugar.Language.Parsing.Nodes.Interfaces.Creation;

namespace Sugar.Language.Parsing.Nodes.Functions.Properties.Accessors
{
    internal abstract class AccessorNode : ParseNodeCollection, ICreationNode, ICreationNode_Body
    {
        protected readonly DescriberNode describer;
        public DescriberNode Describer { get => describer; }

        private readonly ParseNode body;
        public ParseNode Body { get => body; }

        public AccessorNode(DescriberNode _describer, ParseNode _body) : base(_describer, _body)
        {
            describer = _describer;
            body = _body;
        }

        public override ParseNode AddChild(ParseNode node) { return this; }
    }
}
