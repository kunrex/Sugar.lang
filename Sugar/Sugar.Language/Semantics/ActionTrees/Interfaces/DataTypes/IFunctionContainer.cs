using System;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes
{
    internal interface IFunctionContainer<Method, Void> : IBaseFunctionContainer, IContainer<Method, IFunctionContainer<Method, Void>> where Method : IMethodCreation where Void : IVoidCreation
    {
        public IFunctionContainer<Method, Void> AddDeclaration(Void declaration);

        public Void TryFindVoidDeclaration(IdentifierNode identifier, IFunctionArguments arguments);
        public Method TryFindMethodDeclaration(IdentifierNode identifier, IFunctionArguments arguments);
    }
}
