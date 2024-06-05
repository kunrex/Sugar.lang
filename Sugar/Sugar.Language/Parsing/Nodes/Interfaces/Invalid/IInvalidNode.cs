using System;

using Sugar.Language.Exceptions;

namespace Sugar.Language.Parsing.Nodes.Interfaces.Invalid
{
    internal interface IInvalidNode<ExceptionType> where ExceptionType : CompileException
    {
        public ExceptionType Exception { get; }
    }
}
