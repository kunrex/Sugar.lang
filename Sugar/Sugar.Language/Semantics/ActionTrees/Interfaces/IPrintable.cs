using System;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces
{
    internal interface IPrintable
    {
        public void Print(string indent, bool last);
    }
}
