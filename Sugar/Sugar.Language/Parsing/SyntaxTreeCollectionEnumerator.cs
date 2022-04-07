using System;
using System.Collections;
using System.Collections.Generic;

namespace Sugar.Language.Parsing
{
    internal sealed class SyntaxTreeCollectionEnumerator : IEnumerator<SyntaxTree>
    {
        private int position = -1;
        private readonly SyntaxTreeCollection collection;

        public SyntaxTreeCollectionEnumerator(SyntaxTreeCollection _collection)
        {
            collection = _collection;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public SyntaxTree Current
        {
            get
            {
                try
                {
                    return collection[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public bool MoveNext()
        {
            position++;
            return position < collection.Count;
        }

        public void Reset()
        {
            position = -1;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
