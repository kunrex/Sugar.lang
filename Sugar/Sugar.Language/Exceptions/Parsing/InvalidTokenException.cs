using System;

using Sugar.Language.Tokens;

namespace Sugar.Language.Exceptions.Parsing
{
    internal sealed class InvalidTokenException : CompileException
    {
        public InvalidTokenException(Token token) : base($"Invalid Token: '{token.Value}' detected", token.Index)
        {

        }
    }
}
