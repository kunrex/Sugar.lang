using System;

namespace Sugar.Language.Exceptions.Analytics.ClassMemberCreation.OperatorOverloads
{
    internal sealed class BinaryOperationArgumentException : CompileException
    {
        public BinaryOperationArgumentException(int index) : base("At least one argument in a binary operation overload must the data type containing the overload", index)
        {

        }
    }
}
