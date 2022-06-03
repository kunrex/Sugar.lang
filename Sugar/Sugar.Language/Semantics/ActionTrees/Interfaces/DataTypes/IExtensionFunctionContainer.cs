using System;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes
{
    internal interface IExtensionFunctionContainer : IFunctionContainer<ExtensionMethodDeclarationStmt>
    {
        public ExtensionMethodDeclarationStmt TryFindExternalFunctionDeclaration(IdentifierNode identifier);
    }
}
