using System;

using Sugar.Language.Tokens.Operators;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes
{
    internal interface IOperatorContainer : IContainer<OperatorOverloadDeclarationStmt, IOperatorContainer>
    {
        public OperatorOverloadDeclarationStmt TryFindOperatorOverloadDeclaration(Operator op);
    }
}
