using System;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.Interfaces;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation.Local.FunctionArguments;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions
{
    internal interface IFunction : IScopeParent, ICreationStatement
    {
        public Scope Scope { get; }

        public FunctionDeclArgs FunctionArguments { get; }

        public FunctionArgumentDeclarationStmt TryFindFunctionArgument(IdentifierNode identifier);
    }
}
