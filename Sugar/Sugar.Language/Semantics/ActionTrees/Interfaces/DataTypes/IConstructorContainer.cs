﻿using System;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes
{
    internal interface IConstructorContainer : IContainer<ConstructorDeclarationStmt, IConstructorContainer>
    {
        public ConstructorDeclarationStmt TryFindConstructorDeclaration(IdentifierNode identifier, FunctionArguments arguments);
    }
}
