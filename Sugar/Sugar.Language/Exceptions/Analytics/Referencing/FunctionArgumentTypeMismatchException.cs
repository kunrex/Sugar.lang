using System;

namespace Sugar.Language.Exceptions.Analytics.Referencing
{
    internal sealed class FunctionArgumentTypeMismatchException : CompileException
    {
        public FunctionArgumentTypeMismatchException(string function, string arg) : base($"the type of the arg: {arg} in function: {function} is invalid", 0)
        {

        }
    }
}
