using System;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Functions.Properties;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes.Properties;

namespace Sugar.Language.Semantics.ActionTrees.CreatableNodes.PropertyCreation
{
    internal abstract class PropertyCreationNode<BaseNode> : CreationNode<IPropertyContainer<BaseNode>> where BaseNode : PropertyNode
    {
        protected readonly BaseNode baseNode;

        public PropertyCreationNode(DataType _creationType, IdentifierNode _creationName, Describer _describer, BaseNode _baseNode) : base(
            _creationType,
            _creationName,
            _describer,
            DescriberEnum.AccessModifiers | DescriberEnum.Static | DescriberEnum.InheritanceModifiers | DescriberEnum.Override)
        {
            baseNode = _baseNode;
        }
    }
}
