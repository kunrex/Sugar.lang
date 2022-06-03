using System;

using Sugar.Language.Semantics.ActionTrees.Interfaces;

namespace Sugar.Language.Semantics.ActionTrees
{
    internal abstract class ParentableActionTreeNode<T> : ActionTreeNode where T : IActionTreeNode
    {
        public T Parent { get; protected set; }

        public void SetParent() => SetChildrenParent();

        public void SetParent(T _parent)
        {
            Parent = _parent;

            SetChildrenParent();
        }

        protected virtual void SetChildrenParent() { }
    }
}
