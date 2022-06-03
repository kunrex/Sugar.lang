using System;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.PropertyCreation
{
    internal class PropertyDeclarationStmt : PropertyCreationStmt 
    {
        protected readonly Node get;
        public Node GetExpression { get => get; }

        protected readonly Node set;
        public Node SetExpression { get => set; }

        public PropertyTypeEnum PropertyType { get; private set; }

        public PropertyDeclarationStmt(DataType _creationType, IdentifierNode _creationName, Describer _describer, Node _get, Node _set) : base(
            _creationType,
            _creationName,
            _describer)
        {
            get = _get;
            set = _set;

            switch(get)
            {
                case null:
                    PropertyType = PropertyTypeEnum.Set;
                    break;
                default:
                    switch(set)
                    {
                        case null:
                            PropertyType = PropertyTypeEnum.Get;
                            break;
                        default:
                            PropertyType = PropertyTypeEnum.GetSet;
                            break;
                    }
                    break;
            }
        }

        public override string ToString() => $"Property Declaration Node [{creationName.Value}, Type: {PropertyType}]";
    }
}
