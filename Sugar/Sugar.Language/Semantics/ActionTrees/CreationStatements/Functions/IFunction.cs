using System;

using Sugar.Language.Semantics.ActionTrees.Interfaces;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions
{
    internal interface IFunction : IScopeParent
    {
        public Scope Scope { get; }

        public FunctionArguments FunctionArguments { get; }
    }
}
