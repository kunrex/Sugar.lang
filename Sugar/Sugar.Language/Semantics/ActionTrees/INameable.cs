using System;

using Sugar.Language.Semantics.ActionTrees.Interfaces;

namespace Sugar.Language.Semantics.ActionTrees
{
    internal interface INameable : IActionTreeNode
    {
        public string Name { get; }
    }
}
