using System;

namespace Sugar.Language.Exceptions.Analytics.ClassMemberCreation.Statements
{
    internal sealed class InvalidStatementException : CompileException
    {
        public InvalidStatementException(string statement, string expected) : base($"Invalid Statement: '{statement}' detected, {expected} expected.", 0)
        {

        }
    }
}
