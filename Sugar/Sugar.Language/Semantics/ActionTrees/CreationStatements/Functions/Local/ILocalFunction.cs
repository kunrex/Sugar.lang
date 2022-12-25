using System;

using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Local
{
    internal interface ILocalFunction : IFunction, IParentableCreationStatement<IFunctionContainer<LocalMethodCreationStmt, LocalVoidDeclarationStmt>>
    {
       
    }
}
