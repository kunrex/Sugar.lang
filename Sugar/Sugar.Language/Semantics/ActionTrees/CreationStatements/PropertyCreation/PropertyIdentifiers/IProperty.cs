using System;

using Sugar.Language.Semantics.ActionTrees.Enums;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.PropertyCreation.PropertyIdentifiers
{
    internal interface IProperty
    {
        public PropertyTypeEnum PropertyType { get; }

        public PropertyGetIdentifier GetExpression { get; }
        public PropertySetIdentifier SetExpression { get; }
    }
}
