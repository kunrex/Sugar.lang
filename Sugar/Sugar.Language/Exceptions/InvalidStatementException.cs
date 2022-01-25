using System;

namespace Sugar.Language.Exceptions
{
    internal sealed class InvalidStatementException : CompileException
    {
        public InvalidStatementException(int index) : base($"Only assignment, call, declaration, increment, decrement and creation can be used as a statement", index)
        {

        }
    }
}
