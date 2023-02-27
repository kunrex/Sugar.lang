using System;
using System.Collections;
using System.Collections.Generic;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.SyntaxCollections
{
    internal sealed class SyntaxCollectionEnumerator : IEnumerator<SyntaxKind>
    {
        public SyntaxCollection syntaxCollection;

        int position = -1;
        private bool disposed = false;

        public SyntaxCollectionEnumerator(SyntaxCollection _collection)
        {
            syntaxCollection = _collection;
        }

        public bool MoveNext()
        {
            position++;
            return position < syntaxCollection.Count;
        }

        public void Reset()
        {
            position = -1;
        }

        public void Dispose()
        {
            if (disposed)
                return;

            disposed = true;
            GC.SuppressFinalize(this);
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public SyntaxKind Current
        {
            get
            {
                try
                {
                    return syntaxCollection[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}
