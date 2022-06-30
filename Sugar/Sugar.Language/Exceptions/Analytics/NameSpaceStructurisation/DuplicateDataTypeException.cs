using System;

namespace Sugar.Language.Exceptions.Analytics.NameSpaceStructurisation
{
    internal sealed class DuplicateDataTypeException : CompileException
    {
        public DuplicateDataTypeException(string name) : base($"Data Type: {name} already exists", 0)
        {

        }
    }
}
