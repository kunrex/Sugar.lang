using System;

namespace Sugar.Language.Exceptions.Analytics.ClassMemberCreation.Statements
{
    internal sealed class DuplicateGlobalDefinitionException : CompileException
    {
        public DuplicateGlobalDefinitionException(string duplicate, string className) : base($"'{className}' already contains a definition for '{duplicate}'", 0)
        {

        }
    }
}
