using System;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.PropertyCreation.PropertyIdentifiers
{
    internal interface IProperty
    {
        public PropertyGetIdentifier GetExpression { get; }
        public PropertySetIdentifier SetExpression { get; }
    }
}
