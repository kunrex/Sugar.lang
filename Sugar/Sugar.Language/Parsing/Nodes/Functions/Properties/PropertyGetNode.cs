using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Types;

using Sugar.Language.Parsing.Nodes.Describers;

using Sugar.Language.Parsing.Nodes.Functions.Properties.Accessors;

namespace Sugar.Language.Parsing.Nodes.Functions.Properties
{
    internal sealed class PropertyGetNode : PropertyNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.PropertyGet; }

        private readonly GetNode getNode;
        public GetNode Get { get => getNode; }       

        public PropertyGetNode(DescriberNode _describer, TypeNode _type, GetNode _getNode) : base(_describer, _type)
        {
            getNode = _getNode;
            Add(getNode);
        }

        public override string ToString() => $"Only Set property Node";
    }
}
