using System;

using Sugar.Language.Tokens;

namespace Sugar.Language.Exceptions.Parsing
{
    internal sealed class InvalidTokenException : CompileException
    {
        public InvalidTokenException(Token token, int index) : base($"Invalid Token: '{token.Value}' detected", index)
        {

        }
    }
}
