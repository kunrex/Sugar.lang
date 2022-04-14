using System;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes
{
    internal interface IContainer<T, S> : IActionTreeNode where T : IActionTreeNode
    {
        public S AddDeclaration(T declaration);
    }
}
