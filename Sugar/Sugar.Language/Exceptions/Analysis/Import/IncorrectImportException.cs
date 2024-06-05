using System;

namespace Sugar.Language.Exceptions.Analysis.Import
{
    internal sealed class IncorrectImportException : CompileException
    {
        public IncorrectImportException(string name, string imported, string actual) : base($"{name} is not a(n) {imported}. It is a(n) {actual}. Please import the correct type?", 0)
        {
            
        }
    }
}
