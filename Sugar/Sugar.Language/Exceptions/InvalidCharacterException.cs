using System;

namespace Sugar.Language.Exceptions
{
    internal sealed class InvalidCharacterException : CompileException
    {
        public InvalidCharacterException(char character, int index) : base($"Invalid Character: '{character}' detected.", index)
        {

        }

        public InvalidCharacterException(char character, char expected, int index) : base($"Invalid Character: '{character}' detected. '{expected}' expected", index)
        {

        }
    }
}
