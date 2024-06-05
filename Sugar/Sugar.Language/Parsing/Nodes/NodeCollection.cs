using System;
using System.Collections;
using System.Collections.Generic;

using Sugar.Language.Services;
using Sugar.Language.Services.Interfaces;

namespace Sugar.Language.Parsing.Nodes
{
    internal abstract class NodeCollection<Node> : ParseNode, ICustomCollection<Node> where Node : ParseNode
    {
        protected List<Node> Children { get; }
        public int Length { get => Children.Count; }

        public Node this[int index]
        {
            get => index >= Length || index < 0 ? default(Node) : Children[index];
        }

        public NodeCollection()
        {
            Children = new List<Node>();
        }

        public NodeCollection(List<Node> _children)
        {
            Children = _children;
        }

        public NodeCollection(params Node[] _children)
        {
            Children = new List<Node>();

            foreach (var child in _children)
                Children.Add(child);
        }

        protected void Add(Node _node)
        {
            Children.Add(_node);
        }

        public override void SetParent()
        {
            foreach (var child in Children)
                child.SetParent(this);
        }

        public IEnumerator<Node> GetEnumerator()
        {
            return new GenericEnumeratorService<NodeCollection<Node>, Node>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected override void PrintChildren(string indent)
        {
            for (int i = 0; i < Children.Count; i++)
                Children[i].Print(indent, i == Children.Count - 1);
        }
    }
}
