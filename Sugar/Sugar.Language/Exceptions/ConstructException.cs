using System;

namespace Sugar.Language.Exceptions
{
    internal sealed class ConstructException : Exception
    {
        public ConstructException(string singletonPattern) : base($"A secondary instance of {singletonPattern} was created while 'instance ws already asigned")
        {

        }

        public ConstructException(string step, string fileName) : base($"A compile step was not built in the: '{step}' step, in the file name: '{fileName}'")
        {

        }
    }
}
