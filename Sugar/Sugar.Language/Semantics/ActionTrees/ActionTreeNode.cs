using System;

using Sugar.Language.Semantics.ActionTrees.Interfaces;

namespace Sugar.Language.Semantics.ActionTrees
{
    internal abstract class ActionTreeNode : IActionTreeNode, IPrintable
    {
        public void Print(string indent, bool last)
        {
            Console.Write(indent);
            if (last)
            {
                Console.Write("\\-");
                indent += "  ";
            }
            else
            {
                Console.Write("|-");
                indent += "| ";
            }

            Console.WriteLine(ToString());

            PrintChildren(indent);
        }

        protected virtual void PrintChildren(string indent) { }

        public abstract override string ToString();
    }
}
