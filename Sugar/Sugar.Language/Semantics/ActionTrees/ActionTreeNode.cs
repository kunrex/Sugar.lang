using System;
using System.Collections.Generic;

namespace Sugar.Language.Semantics.ActionTrees
{
    internal abstract class ActionTreeNode 
    {
        public ActionTreeNode Parent { get; protected set; }

        protected List<ActionTreeNode> Children { get; set; }

        public int ChildCount => Children.Count;

        public ActionTreeNode this[int index]
        {
            get => index >= ChildCount || index < 0 ? null : Children[index];
        }

        public IEnumerable<ActionTreeNode> GetChildren()
        {
            foreach (var child in Children)
                yield return child;
        }

        public ActionTreeNode()
        {
            Children = new List<ActionTreeNode>();
        }

        public virtual ActionTreeNode AddChild(ActionTreeNode _node)
        {
            Children.Add(_node);

            return this;
        }

        public void SetParent()
        {
            foreach (var child in Children)
                child.SetParent(this);
        }

        public void SetParent(ActionTreeNode _parent)
        {
            Parent = _parent;

            SetParent();
        }

        public abstract override string ToString();

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

            for (int i = 0; i < Children.Count; i++)
                Children[i].Print(indent, i == Children.Count - 1);
        }
    }
}
