using System;

namespace Sugar.Language.Exceptions.Analysis.Import
{
    internal sealed class NoReferenceFoundException : CompileException
    {
        public NoReferenceFoundException() : base("No such reference exists", 0)
        {
        }
    }
}
