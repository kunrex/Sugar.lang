using System;
using System.Collections;
using System.Collections.Generic;

namespace Sugar.Language.Parsing
{
    internal sealed class SyntaxTreeCollection : IEnumerable<SyntaxTree>
    {
        private readonly List<SyntaxTree> Trees;

        public SyntaxTree this[int index]
        {
            get => Trees[index];
        }

        public int Count { get => Trees.Count; }

        public SyntaxTreeCollection()
        {
            Trees = new List<SyntaxTree>();
        }

        public void Add(SyntaxTree tree)
        {
            Trees.Add(tree);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<SyntaxTree> GetEnumerator()
        {
            return new SyntaxTreeCollectionEnumerator(this);
        }
    }
}
