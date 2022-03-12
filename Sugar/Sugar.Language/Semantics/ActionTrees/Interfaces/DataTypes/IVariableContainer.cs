using System;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Semantics.ActionTrees.VariableCreation;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes
{
    internal interface IVariableContainer : IActionTreeNode
    {
        public GlobalVariableDeclaration TryFindDeclaration(IdentifierNode identifier);
        public IVariableContainer AddDeclaration(GlobalVariableDeclaration declaration);
    }
}
