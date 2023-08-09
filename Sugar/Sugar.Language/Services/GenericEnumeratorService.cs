using System;
using System.Collections;
using System.Collections.Generic;
using Sugar.Language.Services.Interfaces;

namespace Sugar.Language.Services
{
    internal sealed class GenericEnumeratorService<Collection, Enumerable> : IEnumerator<Enumerable> where Collection : IEnumerable<Enumerable>, ICustomCollection<Enumerable>
    {
        int position = -1;
        private readonly Collection collection;

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public Enumerable Current
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

        public GenericEnumeratorService(Collection _collection)
        {
            collection = _collection;
        }

        public bool MoveNext()
        {
            position++;
            return position < collection.Length;
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
