using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Types;

using Sugar.Language.Parsing.Nodes.Describers;

using Sugar.Language.Parsing.Nodes.Functions.Properties.Accessors;

namespace Sugar.Language.Parsing.Nodes.Functions.Properties
{
    internal sealed class PropertyGetSetNode : PropertyNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.PropertyGetSet; }

        private readonly GetNode getNode;
        public GetNode Get { get => getNode; }

        private readonly SetNode setNode;
        public SetNode Set { get => setNode; }

        public PropertyGetSetNode(DescriberNode _describer, TypeNode _type, GetNode _getNode, SetNode _setNode) : base(_describer, _type)
        {
            getNode = _getNode;
            Add(getNode);

            setNode = _setNode;
            Add(setNode);
        }

        public override string ToString() => $"Get Set Property Node";
    }
}
