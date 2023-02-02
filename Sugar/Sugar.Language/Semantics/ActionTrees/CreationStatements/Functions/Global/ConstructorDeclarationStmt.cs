using System;

using Sugar.Language.Parsing.Nodes;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global
{
    internal sealed class ConstructorDeclarationStmt : GlobalMethodCreationStmt<IConstructorContainer>
    {
        public override ActionNodeEnum ActionNodeType { get => ActionNodeEnum.Constructor; }

        public ConstructorDeclarationStmt(DataType _creationType, Describer _describer, FunctionDeclArgs _arguments, Node _nodeBody) : base(
            _creationType,
            _creationType.Name,
            _describer,
            _arguments,
            _nodeBody)
        {

        }

        public override string ToString() => "Constructor Declration Statement";
    }
}
