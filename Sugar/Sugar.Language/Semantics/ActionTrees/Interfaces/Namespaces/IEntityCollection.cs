using System;

using Sugar.Language.Parsing.Nodes.Values;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces.Namespaces
{
    internal interface IEntityCollection<T, K> : IActionTreeNode
    {
        public K AddEntity(T dataType);
    }
}
