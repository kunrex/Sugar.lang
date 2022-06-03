using System;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes
{
    internal interface IVariableContainer : IContainer<VariableCreationStmt, IVariableContainer>
    {
        public VariableCreationStmt TryFindVariableCreation(IdentifierNode identifier);
    }
}
