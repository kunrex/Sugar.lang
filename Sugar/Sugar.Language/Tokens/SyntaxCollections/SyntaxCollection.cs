using System;
using System.Collections;
using System.Collections.Generic;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.SyntaxCollections
{
    internal sealed partial class SyntaxCollection : IReadOnlyCollection<SyntaxKind>
    {
        private readonly SyntaxKind[] collection;
        public int Count { get => collection.Length; }

        private SyntaxCollection(params SyntaxKind[] _collection)
        {
            collection = _collection;
        }

        public SyntaxKind this[int index]
        {
            get => collection[index];
        }

        public bool Contains(SyntaxKind kind)
        {
            foreach (var syntax in collection)
                if (syntax == kind)
                    return true;

            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<SyntaxKind> GetEnumerator()
        {
            return new SyntaxCollectionEnumerator(this);
        }
    }
}
