using System;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes
{
    internal interface IFunctionContainer<T> : IContainer<T, IFunctionContainer<T>> where T : IMethodCreation
    {
        public T TryFindFunctionDeclaration(IdentifierNode identifier);
    }
}
