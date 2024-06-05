using System;

using Sugar.Language.Parsing.Nodes.Types;

using Sugar.Language.Parsing.Nodes.Describers;

using Sugar.Language.Parsing.Nodes.Interfaces.Creation;

namespace Sugar.Language.Parsing.Nodes.Functions.Properties
{
    internal abstract class PropertyNode : ParseNodeCollection, ICreationNode_Type
    {
        protected readonly DescriberNode describer;
        public DescriberNode Describer { get => describer; }

        protected readonly TypeNode type;
        public TypeNode Type { get => type; }

        public PropertyNode(DescriberNode _describer, TypeNode _type) : base(_describer, _type)
        {
            describer = _describer;

            type = _type;
        }

        public override ParseNode AddChild(ParseNode node) { return this; }
    }
}
