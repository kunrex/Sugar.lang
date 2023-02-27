using System;

using Sugar.Language.Parsing.Nodes;

using Sugar.Language.Exceptions;

namespace Sugar.Language.Parsing.Parser.Structure
{
    internal struct ParseGenericResult
    {
        public bool Success { get => Generic != null; }

        public Node Generic { get; private set; }
        public CompileException Exception { get; private set; }

        public ParseGenericResult(Node node, CompileException exception)
        {
            Generic = node;
            Exception = exception;
        }
    }
}
