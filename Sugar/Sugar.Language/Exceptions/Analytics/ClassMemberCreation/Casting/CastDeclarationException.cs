using System;

namespace Sugar.Language.Exceptions.Analytics.ClassMemberCreation.Casting
{
    internal sealed class CastDeclarationException : CompileException
    {
        public CastDeclarationException(int index) : base("Either the argument or the return type of the conversion operator must be the data type defining it", index)
        {

        }
    }
}
