using System;

using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global
{
    internal interface IGlobalFunction : IFunction, IParentableCreationStatement<IFunctionContainer<MethodDeclarationStmt, VoidDeclarationStmt>>
    {
        
    }
}
