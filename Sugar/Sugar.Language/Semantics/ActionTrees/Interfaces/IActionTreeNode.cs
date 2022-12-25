using System;

using Sugar.Language.Semantics.ActionTrees.Enums;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces
{
    internal interface IActionTreeNode
    {
        public ActionNodeEnum ActionNodeType { get; }
    }
}
