using System;

using Sugar.Language.Parsing.Nodes.Functions.Declarations.OperatorOverloading;

using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes.Casts;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global.Conversion
{
    internal sealed class ImplicitCastDeclarationStmt : CastDeclarationStmt<ImplicitCastDeclarationNode, IImplicitContainer>
    {
        public ImplicitCastDeclarationStmt(DataType _creationType, Describer _describer, FunctionArguments _arguments, ImplicitCastDeclarationNode _baseNode) : base(
           _creationType,
           _describer,
           _arguments,
           _baseNode)
        {

        }

        public override string ToString() => $"Implicit Cast Declaration Node";
    }
}
