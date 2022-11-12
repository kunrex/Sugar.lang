using System;

using Sugar.Language.Semantics.ActionTrees.Interfaces;

using Sugar.Language.Exceptions.Analytics.Processing;

namespace Sugar.Language.Semantics.ActionTrees
{
    internal abstract class ActionTreeNode<ParentType> : IParentableActionTreeNode<ParentType>, IPrintable where ParentType : IActionTreeNode
    {
        public ParentType Parent { get; private set; }

        public void SetParent(ParentType parent)
        {
            if (Parent != null)
                throw new DoubleParentAssignementException();

            Parent = parent;
        }

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

        public abstract override string ToString();

        protected virtual void PrintChildren(string indent) { }
    }
}
