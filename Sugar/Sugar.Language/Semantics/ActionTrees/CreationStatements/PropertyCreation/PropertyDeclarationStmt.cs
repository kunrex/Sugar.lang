﻿using System;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.PropertyCreation.PropertyIdentifiers;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.PropertyCreation
{
    internal class PropertyDeclarationStmt : PropertyCreationStmt 
    {
        protected readonly PropertyGetIdentifier get;
        public override PropertyGetIdentifier GetExpression { get => get; }

        protected readonly PropertySetIdentifier set;
        public override PropertySetIdentifier SetExpression { get => set; }

        public override ActionNodeEnum ActionNodeType { get => (ActionNodeEnum)PropertyType; }

        public PropertyDeclarationStmt(DataType _creationType, IdentifierNode _creationName, Describer _describer, PropertyGetIdentifier _get, PropertySetIdentifier _set) : base(
            _creationType,
            _creationName,
            _describer)
        {
            if (get != null)
            {
                get = _get;
                PropertyType = PropertyTypeEnum.Get;
            }

            if (set != null)
            {
                set = _set;
                PropertyType = PropertyType == PropertyTypeEnum.Get ? PropertyTypeEnum.GetSet : PropertyTypeEnum.Set;
            }
        }

        public override string ToString() => $"Property Declaration Node [{creationName}, Type: {PropertyType}]";
    }
}
