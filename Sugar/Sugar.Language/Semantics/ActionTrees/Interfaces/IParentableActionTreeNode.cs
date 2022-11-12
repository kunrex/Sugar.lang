using System;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces
{
    internal interface IParentableActionTreeNode<ParentType> where ParentType : IActionTreeNode
    {
        public ParentType Parent { get; }

        public void SetParent(ParentType parent);
    }
}
