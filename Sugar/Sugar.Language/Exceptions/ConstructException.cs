using System;

namespace Sugar.Language.Exceptions
{
    internal sealed class ConstructException : Exception
    {
        public ConstructException(string step, string fileName) : base($"A compile step was not built in the: '{step}' step, in the file name: '{fileName}'")
        {

        }
    }
}
