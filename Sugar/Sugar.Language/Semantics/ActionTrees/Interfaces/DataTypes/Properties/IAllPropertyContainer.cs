using System;

using Sugar.Language.Parsing.Nodes.Functions.Properties;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes.Properties
{
    internal interface IAllPropertyContainer : IPropertyContainer<PropertyGetNode>, IPropertyContainer<PropertySetNode>, IPropertyContainer<PropertyGetSetNode>
    {

    }
}
