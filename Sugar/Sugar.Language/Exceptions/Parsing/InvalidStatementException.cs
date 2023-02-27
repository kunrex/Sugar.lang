using System;

namespace Sugar.Language.Exceptions.Parsing
{
    internal sealed class InvalidStatementException : CompileException
    {
        public InvalidStatementException(int index) : base($"Only assignment, call, increment, decrement and object creation can stand alone as a statement", index)
        {

        }
    }
}
