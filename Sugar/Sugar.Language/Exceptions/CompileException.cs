using System;
namespace Sugar.Language.Exceptions
{
    internal abstract class CompileException : Exception
    {
        public CompileException(string exception, int index) : base($"{exception}. At {index}")
        {

        }
    }
}
