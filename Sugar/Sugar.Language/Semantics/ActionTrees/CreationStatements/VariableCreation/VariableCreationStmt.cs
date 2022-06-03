using System;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation
{
    internal abstract class VariableCreationStmt : CreationStatement<IVariableContainer>
    {
        protected readonly bool isLocal;

        public VariableCreationStmt(DataType _creationType, IdentifierNode _creationName, Describer _describer, bool _isLocal) : base(
            _creationType,
            _creationName,
            _describer,
            _isLocal ? DescriberEnum.AccessModifiers | DescriberEnum.Static | DescriberEnum.MutabilityModifier : DescriberEnum.MutabilityModifier)
        {
            isLocal = _isLocal;
        }
    }
}
