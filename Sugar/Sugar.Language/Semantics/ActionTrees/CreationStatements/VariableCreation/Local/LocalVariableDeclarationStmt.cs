using System;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation.Local
{
    internal class LocalVariableDeclarationStmt : VariableCreationStmt<LocalVariableDeclarationStmt, ILocalVariableContainer>
    {
        public override CreationTypeEnum CreationEnumType { get => CreationTypeEnum.LocalVariable; }

        public LocalVariableDeclarationStmt(DataType _creationType, IdentifierNode _creationName, Describer _describer, DescriberEnum _addOn = 0) : base(
            _creationType,
            _creationName,
            _describer,
             DescriberEnum.MutabilityModifier | _addOn)
        {

        }

        public override string ToString() => $"Local Variable Declaraion [{creationName}";
    }
}
