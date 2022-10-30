using System;

using Sugar.Language.Parsing.Nodes;

using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Local;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions
{
    internal interface IFunction : IFunctionContainer<LocalMethodCreationStmt, LocalVoidDeclarationStmt>
    {
        public Scope Scope { get; }

        public FunctionArguments FunctionArguments { get; }
    }
}
