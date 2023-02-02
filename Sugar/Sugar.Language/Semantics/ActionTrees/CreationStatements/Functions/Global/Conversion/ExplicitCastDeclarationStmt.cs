using System;

using Sugar.Language.Parsing.Nodes;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes.Casts;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global.Conversion
{
    internal sealed class ExplicitCastDeclarationStmt : CastDeclarationStmt<IExplicitContainer>
    {
        public override ActionNodeEnum ActionNodeType { get => ActionNodeEnum.ExplicitCast; }

        public ExplicitCastDeclarationStmt(DataType _creationType, Describer _describer, FunctionDeclArgs _arguments, Node _nodeBody) : base(
            _creationType,
            _describer,
            _arguments,
            _nodeBody)
        {

        }

        public override string ToString() => $"Explicit Cast Declaration Node";
    }
}
