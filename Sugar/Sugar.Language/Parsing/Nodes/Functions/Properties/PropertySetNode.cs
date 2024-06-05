using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Types;

using Sugar.Language.Parsing.Nodes.Describers;

using Sugar.Language.Parsing.Nodes.Functions.Properties.Accessors;

namespace Sugar.Language.Parsing.Nodes.Functions.Properties
{
    internal sealed class PropertySetNode : PropertyNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.PropertySet; }

        private readonly SetNode setNode;
        public SetNode Set { get => setNode; }

        public PropertySetNode(DescriberNode _describer, TypeNode _type, SetNode _setNode) : base(_describer, _type)
        {
            setNode = _setNode;
            Add(setNode);
        }

        public override string ToString() => $"Only Set property Node";
    }
}
