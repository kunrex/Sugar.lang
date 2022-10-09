using System;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.PropertyCreation
{
    internal abstract class PropertyCreationStmt : ReturnableCreationStatement<IPropertyContainer> 
    {
        protected readonly IdentifierNode identifier;

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
