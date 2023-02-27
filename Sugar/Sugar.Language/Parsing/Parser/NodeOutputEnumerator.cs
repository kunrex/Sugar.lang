using System;
using System.Collections;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes;

namespace Sugar.Language.Parsing.Parser
{
    internal sealed class NodeOutputEnumerator : IEnumerator<Node>
    {
        public NodeOutput nodeOuptut;

        int position = -1;
        private bool disposed = false;

        public NodeOutputEnumerator(NodeOutput _collection)
        {
            nodeOuptut = _collection;
        }

        public bool MoveNext()
        {
            position++;
            return position < nodeOuptut.OutputCount;
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

        public Node Current
        {
            get
            {
                try
                {
                    return nodeOuptut[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}
