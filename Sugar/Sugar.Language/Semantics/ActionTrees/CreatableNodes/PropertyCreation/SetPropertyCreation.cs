using System;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Functions.Properties;

using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;

namespace Sugar.Language.Semantics.ActionTrees.CreatableNodes.PropertyCreation
{
    internal sealed class SetPropertyCreation : PropertyCreationNode<PropertySetNode>
    {
        public Node SetNode { get => baseNode.Set; }

        public SetPropertyCreation(DataType _creationType, IdentifierNode _creationName, Describer _describer, PropertySetNode _setNode) : base(
            _creationType,
            _creationName,
            _describer,
            _setNode)
        {
           
        }

        public override string ToString() => $"Set Property Creation Node [Name: {creationName.Value}]";
    }
}
