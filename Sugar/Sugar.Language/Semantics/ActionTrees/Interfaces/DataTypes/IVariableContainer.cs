using System;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes
{
    internal interface IVariableContainer : IContainer<GlobalVariableDeclarationStmt, IVariableContainer>
    {
        public GlobalVariableDeclarationStmt TryFindVariableCreation(IdentifierNode identifier);
    }
}
