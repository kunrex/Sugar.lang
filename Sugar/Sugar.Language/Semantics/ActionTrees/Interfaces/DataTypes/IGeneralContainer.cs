using System;

using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes.Casts;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes
{
    internal interface IGeneralContainer : IVariableContainer, IConstructorContainer, IPropertyContainer, IFunctionContainer<MethodDeclarationStmt, VoidDeclarationStmt>, IOperatorContainer, IIndexerContainer, IImplicitContainer, IExplicitContainer
    {

    }
}
