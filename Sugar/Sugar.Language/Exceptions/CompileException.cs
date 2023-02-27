using System;

namespace Sugar.Language.Exceptions
{
    internal abstract class CompileException : Exception
    {
        protected readonly int index0;
        public int Index0 { get => index0; }

        protected readonly int index1;
        public int Index1 { get => index1; }

        protected readonly string expcetionString;
        public string ExceptionString { get => expcetionString; }

        public CompileException(string exception, int index) : base($"{exception}.\n At {index}")
        {
            expcetionString = $"{exception}.\n At {index}";

            index0 = index;
            index1 = index + exception.Length;
        }

        public override string ToString() => expcetionString;
    }
}
