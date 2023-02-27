using System;

using Sugar.Language.Parsing.Nodes;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.PropertyCreation;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.PropertyCreation.PropertyIdentifiers;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global
{
    internal sealed class IndexerCreationStmt : GlobalMethodCreationStmt<IIndexerContainer>, IProperty
    {
        private readonly PropertyCreationStmt property;

        public PropertyGetIdentifier GetExpression { get => property.GetExpression; }
        public PropertySetIdentifier SetExpression { get => property.SetExpression; }

        public PropertyTypeEnum PropertyType { get => property.PropertyType; }

        public override ActionNodeEnum ActionNodeType { get => (ActionNodeEnum)PropertyType; }

        public IndexerCreationStmt(DataType _creationType, Describer _describer, FunctionDeclArgs _arguments, PropertyCreationStmt _property) : base(
            _creationType,
            _creationType.Name,
            _describer,
            _arguments,
            null)
        {
            property = _property;
        }

        public override string ToString() => $"Indexer Declaration Node";
    }
}
