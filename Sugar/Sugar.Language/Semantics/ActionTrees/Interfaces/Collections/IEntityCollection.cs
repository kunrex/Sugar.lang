using System;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces.Collections
{
    internal interface IEntityCollection<T, K> : IActionTreeNode
    {
        public K AddEntity(T dataType);
    }
}
