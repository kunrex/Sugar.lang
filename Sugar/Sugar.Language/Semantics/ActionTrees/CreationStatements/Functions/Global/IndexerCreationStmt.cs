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
        private readonly PropertyGetIdentifier get;
        public PropertyGetIdentifier GetExpression { get => get; }

        private readonly PropertySetIdentifier set;
        public PropertySetIdentifier SetExpression { get => set; }

        public PropertyTypeEnum PropertyType { get; private set; }

        public override ActionNodeEnum ActionNodeType { get => (ActionNodeEnum)PropertyType; }

        public IndexerCreationStmt(DataType _creationType, Describer _describer, FunctionDeclArgs _arguments, Node _get, Node _set) : base(
            _creationType,
            _creationType.Name,
            _describer,
            _arguments,
            null)
        {
            if (get != null)
            {
                get = new PropertyGetIdentifier(_get);
                PropertyType = PropertyTypeEnum.Get;
            }

            if (set != null)
            {
                set = new PropertySetIdentifier(_set, _creationType);
                PropertyType = PropertyType == PropertyTypeEnum.Get ? PropertyTypeEnum.GetSet : PropertyTypeEnum.Set;
            }
        }

        public override string ToString() => $"Indexer Declaration Node";
    }
}
