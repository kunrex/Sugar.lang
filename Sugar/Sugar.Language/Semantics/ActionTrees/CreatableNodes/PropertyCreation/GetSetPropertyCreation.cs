using System;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Functions.Properties;

using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;

namespace Sugar.Language.Semantics.ActionTrees.CreatableNodes.PropertyCreation
{
    internal sealed class GetSetPropertyCreation : PropertyCreationNode<PropertyGetSetNode>
    {
        public Node GetNode { get => baseNode.Get; }
        public Node SetNode { get => baseNode.Set; }

        public GetSetPropertyCreation(DataType _creationType, IdentifierNode _creationName, Describer _describer, PropertyGetSetNode _getSetNode) : base(
            _creationType,
            _creationName,
            _describer,
            _getSetNode)
        {

        }

        public override string ToString() => $"Get Set Property Creation Node [Name: {creationName.Value}]";
    }
}
