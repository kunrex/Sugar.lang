using System;

namespace Sugar.Language.Exceptions.Analytics.Processing
{
    internal sealed class DoubleParentAssignementException : CompileException
    {
        public DoubleParentAssignementException() : base($"Duplicate Parent Assignement", -1)
        {

        }
    }
}
