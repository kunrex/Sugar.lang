using System;

using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global.Conversion;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes.Casts
{
    internal interface IImplicitContainer : ICastContainer<ImplicitCastDeclarationStmt, IImplicitContainer>
    {
        public ImplicitCastDeclarationStmt TryFindImplicitCastDeclaration(DataType external);
    }
}
