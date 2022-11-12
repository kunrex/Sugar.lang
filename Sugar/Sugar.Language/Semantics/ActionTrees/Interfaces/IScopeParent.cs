using System;

using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Local;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces
{
    internal interface IScopeParent : IFunctionContainer<LocalMethodCreationStmt, LocalVoidDeclarationStmt>, ILocalVariableContainer
    {
        
    }
}
