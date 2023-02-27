using System;
using System.Collections.Generic;

using Sugar.Language.Tokens;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes
{
    internal abstract class Node
    {
        public Node Parent { get; protected set; }

        protected List<Node> Children { get; set; }
        public abstract NodeType NodeType { get; }

        public int ChildCount => Children.Count;

        public Node this[int index]
        {
            get => index >= ChildCount || index < 0 ? null : Children[index];
        }

        public IEnumerable<Node> GetChildren()
        {
            foreach (var child in Children)
                yield return child;
        }

        public Node()
        { 
            Children = new List<Node>();
        }

        public Node(List<Node> children)
        {
            Children = children;
        }

        public virtual Node AddChild(Node _node)
        {
            Children.Add(_node);

            return this;
        }

        public void SetParent()
        {
            foreach (var child in Children)
                child.SetParent(this);
        }

        public void SetParent(Node _parent)
        {
            Parent = _parent;

            SetParent();
        }

        public IEnumerable<Node> GetChildrenBefore(int index)
        {
            for (int i = 0; i < index; i++)
                yield return Children[i];
        }

        public IEnumerable<Node> GetChildrenBefore(Node node)
        {
            var index = Children.IndexOf(node);

            if (index == -1)
                yield return null;

            foreach (var child in GetChildrenBefore(index))
                yield return child;
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
