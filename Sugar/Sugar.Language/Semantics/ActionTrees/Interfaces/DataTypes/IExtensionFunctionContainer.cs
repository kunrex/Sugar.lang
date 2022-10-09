using System;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global.Extensions;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes
{
    internal interface IExtensionFunctionContainer : IBaseFunctionContainer, IContainer<ExtensionMethodDeclarationStmt, IFunctionContainer<ExtensionMethodDeclarationStmt, ExtensionVoidDeclarationStmt>> 
    {
        public IFunctionContainer<ExtensionMethodDeclarationStmt, ExtensionVoidDeclarationStmt> AddExtensionVoidDeclaration(ExtensionVoidDeclarationStmt declaration);

        public ExtensionVoidDeclarationStmt TryFindExtensionMethodDeclaration(IdentifierNode identifier);
        public ExtensionMethodDeclarationStmt TryFindExtensionFunctionDeclaration(IdentifierNode identifier);
    }
}
