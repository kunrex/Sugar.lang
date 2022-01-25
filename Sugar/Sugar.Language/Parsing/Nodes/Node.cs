using System;
using System.Linq;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes
{
    internal abstract class Node
    {
        protected List<Node> Children { get; set; }
        public abstract NodeType NodeType { get; }

        public int ChildCount => Children.Count;

        public Node this[int index]
        {
            get => ChildCount >= index || index < 0 ? null : Children[index];
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
