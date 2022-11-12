using System;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation.Local;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes
{
    internal interface ILocalVariableContainer : IContainer<LocalVariableDeclarationStmt, ILocalVariableContainer>
    {
        public LocalVariableDeclarationStmt TryFindVariableCreation(IdentifierNode identifier);
    }
}
