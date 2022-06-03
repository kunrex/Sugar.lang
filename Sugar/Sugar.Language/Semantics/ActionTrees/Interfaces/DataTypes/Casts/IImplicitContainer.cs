using System;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Functions.Declarations.OperatorOverloading;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global.Conversion;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes.Casts
{
    internal interface IImplicitContainer : ICastContainer<ImplicitCastDeclarationStmt, ImplicitCastDeclarationNode, IImplicitContainer>
    {
        public ImplicitCastDeclarationStmt TryFindImplicitCastDeclaration(IdentifierNode identifier);
    }
}
