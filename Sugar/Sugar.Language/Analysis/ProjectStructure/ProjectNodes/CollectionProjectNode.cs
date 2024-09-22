using System;
using System.Collections;
using System.Collections.Generic;

using Sugar.Language.Services;
using Sugar.Language.Services.Interfaces;

namespace Sugar.Language.Analysis.ProjectStructure.ProjectNodes
{
    internal abstract class CollectionProjectNode<Node> : ProjectNode, ICustomCollection<Node> where Node : ProjectNode
    {
        protected readonly Dictionary<string, Node> children;
        public int Length { get => children.Count; }

        public abstract Node this[int index] { get; }

        public CollectionProjectNode(string _name) : base(_name)
        {
            children = new Dictionary<string, Node>();
        }

        public IEnumerator<Node> GetEnumerator()
        {
            return new GenericEnumeratorService<CollectionProjectNode<Node>, Node>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
