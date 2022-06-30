using System;

namespace Sugar.Language.Exceptions.Analytics.NameSpaceStructurisation
{
    internal sealed class InvalidStatementException : CompileException
    {
        public InvalidStatementException(string location, string expected) : base($"Invalid statement in {location}, {expected} expected.", 0)
        {

        }
    }
}
