using System;

using Sugar.Language.Parsing.Nodes.Functions.Properties;

using Sugar.Language.Semantics.ActionTrees.CreatableNodes.PropertyCreation;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes.Properties
{
    internal interface IPropertyContainer<T> : IContainer<PropertyCreationNode<T>, IPropertyContainer<T>> where T : PropertyNode
    {
        
    }
}
