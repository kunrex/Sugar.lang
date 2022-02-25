using System;

namespace Sugar.Language.Semantics.ActionTrees
{
    internal abstract class ActionTreeNode 
    {
        public ActionTreeNode Parent { get; protected set; }

        public void SetParent() => SetChildrenParent();

        public void SetParent(ActionTreeNode _parent)
        {
            Parent = _parent;

            SetParent();
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

        protected virtual void SetChildrenParent() { }
        protected virtual void PrintChildren(string indent) { }
    }
}
