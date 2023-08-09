using System;

namespace Sugar.Language.Exceptions.Parsing
{
    internal sealed class InvalidAssignmentException : CompileException
    {
        public InvalidAssignmentException(int index) : base("Invalid Assignement operation. Only non constant variables can be assigned to,", index)
        {

        }
    }
}
