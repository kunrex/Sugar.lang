using System;

namespace Sugar.Language.Exceptions.Analysis.Project
{
    internal sealed class DuplicateDataTypeException : CompileException
    {
        public DuplicateDataTypeException(string name) : base($"Data Type: {name} already exists", 0)
        {

        }
    }
}
