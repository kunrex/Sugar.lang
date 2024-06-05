using System;

using Sugar.Language.Exceptions;

using Sugar.Language.Parsing.Nodes.Values.Generics;

namespace Sugar.Language.Parsing.Parser.Structure
{
    internal struct ParseGenericResult
    {
        public bool Success { get => generic != null; }

        private CompileException exception;
        public CompileException Exception { get => exception; }

        private GenericCallNode generic;
        public GenericCallNode Generic { get => generic; }

        public ParseGenericResult(CompileException _exception)
        {
            generic = null;
            exception = _exception;
        }

        public ParseGenericResult(GenericCallNode _generic)
        {
            generic = _generic;
            exception = null;
        }
    }
}
