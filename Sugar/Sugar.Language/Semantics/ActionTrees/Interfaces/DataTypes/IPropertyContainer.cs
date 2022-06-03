using System;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.CreationStatements.PropertyCreation;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes
{
    internal interface IPropertyContainer : IContainer<PropertyCreationStmt, IPropertyContainer>
    {
        public PropertyCreationStmt TryFindpropertyCreation(IdentifierNode identifier);
    }
}
