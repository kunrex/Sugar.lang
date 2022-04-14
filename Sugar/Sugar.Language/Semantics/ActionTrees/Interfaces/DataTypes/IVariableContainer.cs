using System;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Semantics.ActionTrees.CreatableNodes.VariableCreation;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes
{
    internal interface IVariableContainer : IContainer<GlobalVariableDeclaration, IVariableContainer>
    {
        public GlobalVariableDeclaration TryFindVariableDeclaration(IdentifierNode identifier);
    }
}
