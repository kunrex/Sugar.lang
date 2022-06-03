using System;

namespace Sugar.Language.Exceptions
{
    internal abstract class CompileException : Exception
    {
        protected readonly string expcetionString;

        public CompileException(string exception, int index) : base($"{exception}.\n At {index}")
        {
            expcetionString = $"{exception}.\n At {index}";
        }

        public override string ToString() => expcetionString;
    }
}
