using System;

namespace Sugar.Language.Exceptions.Analytics.ClassMemberCreation.Casting
{
    internal sealed class CastDeclarationArgumentsException : CompileException
    {
        public CastDeclarationArgumentsException(int index) : base("A cast declaration can only have one argument", index)
        {

        }
    }
}
