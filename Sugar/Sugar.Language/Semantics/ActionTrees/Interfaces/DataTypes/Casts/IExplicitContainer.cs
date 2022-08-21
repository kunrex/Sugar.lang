using System;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global.Conversion;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes.Casts
{
    internal interface IExplicitContainer : ICastContainer<ExplicitCastDeclarationStmt, IExplicitContainer>
    {
        public ExplicitCastDeclarationStmt TryFindExplicitCastDeclaration(IdentifierNode identifier);
    }
}
