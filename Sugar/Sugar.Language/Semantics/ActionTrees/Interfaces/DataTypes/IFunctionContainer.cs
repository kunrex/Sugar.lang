﻿using System;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes
{
    internal interface IFunctionContainer<Function, Void> : IBaseFunctionContainer, IContainer<Function, IFunctionContainer<Function, Void>> where Function : IMethodCreation where Void : IVoidCreation
    {
        public IFunctionContainer<Function, Void> AddDeclaration(Void declaration);

        public Void TryFindMethodDeclaration(IdentifierNode identifier, FunctionArguments arguments);
        public Function TryFindFunctionDeclaration(IdentifierNode identifier, FunctionArguments arguments);
    }
}
