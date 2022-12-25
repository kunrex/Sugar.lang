using System;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.PropertyCreation.PropertyIdentifiers;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.PropertyCreation
{
    internal interface IProperty : IReferencableEntity
    {
        public PropertyTypeEnum PropertyType { get; }

        public PropertyGetIdentifier GetExpression { get; }
        public PropertySetIdentifier SetExpression { get; }
    }
}
