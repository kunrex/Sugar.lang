using System;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation
{
    internal class GlobalVariableDeclarationStmt : VariableCreationStmt<GlobalVariableDeclarationStmt, IVariableContainer>
    {
        public override ActionNodeEnum ActionNodeType { get => ActionNodeEnum.GlobalVariable; }

        public GlobalVariableDeclarationStmt(DataType _creationType, IdentifierNode _creationName, Describer _describer) : base(
            _creationType,
            _creationName,
            _describer,
            DescriberEnum.AccessModifiers | DescriberEnum.Static | DescriberEnum.MutabilityModifier)
        {
            
        }

        public override string ToString() => $"Global Variable Declaraion [{creationName}";
    }
}
