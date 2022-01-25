using System;

using Sugar.Language.Parsing.Nodes;

namespace Sugar.Language.Parsing.Parser.Structure
{
    internal struct ParseGenericResult
    {
        public bool Success { get => Generic != null; }

        public Node Generic { get; private set; }
        public Exception Exception { get; private set; }

        public ParseGenericResult(Node node, Exception exception)
        {
            Generic = node;
            Exception = exception;
        }
    }
}
