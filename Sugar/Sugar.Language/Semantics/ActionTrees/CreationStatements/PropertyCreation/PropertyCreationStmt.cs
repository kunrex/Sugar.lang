using System;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.PropertyCreation.PropertyIdentifiers;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.PropertyCreation
{
    internal abstract class PropertyCreationStmt : ReturnableCreationStatement<IPropertyContainer>, IProperty
    {
        protected readonly IdentifierNode identifier;

        public abstract PropertyGetIdentifier GetExpression { get; }
        public abstract PropertySetIdentifier SetExpression { get; }

        public PropertyTypeEnum PropertyType { get; protected set; }

        public PropertyCreationStmt(DataType _creationType, IdentifierNode _creationName, Describer _describer) : base(
            _creationType,
            _creationName.Value,
            _describer,
            DescriberEnum.AccessModifiers | DescriberEnum.Static | DescriberEnum.InheritanceModifiers | DescriberEnum.Override)
        {
            identifier = _creationName;
        }
    }
}
