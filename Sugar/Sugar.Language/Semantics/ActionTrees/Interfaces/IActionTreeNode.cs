using System;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces
{
    internal interface IActionTreeNode
    {
        public void Print(string indent, bool last);
    }
}
