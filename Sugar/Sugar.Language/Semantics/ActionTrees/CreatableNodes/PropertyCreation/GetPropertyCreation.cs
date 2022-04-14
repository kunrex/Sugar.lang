using System;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Functions.Properties;

using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;

namespace Sugar.Language.Semantics.ActionTrees.CreatableNodes.PropertyCreation
{
    internal sealed class GetPropertyCreation : PropertyCreationNode<PropertyGetNode>
    {
        public Node GetNode { get => baseNode.Get; }

        public GetPropertyCreation(DataType _creationType, IdentifierNode _creationName, Describer _describer, PropertyGetNode _getNode) : base(
            _creationType,
            _creationName,
            _describer,
            _getNode)
        {

        }

        public override string ToString() => $"Get Property Creation Node [Name: {creationName.Value}]";
    }
}
