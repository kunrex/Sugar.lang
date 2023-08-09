using System;

using Sugar.Language.Tokens;
using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Exceptions.Parsing
{
    internal sealed class TokenExpectedException : CompileException
    {
        public TokenExpectedException(TokenType expected, int index) : base($"Invlaid Token detected, {expected} epxected", index)
        {

        }

        public TokenExpectedException(TokenType expected, Token token) : base($"Invlaid Token: '{token.Value}' detected, {expected} epxected", token.Index)
        {

        }

        public TokenExpectedException(Token expected, int index) : base($"Invlaid Token detected, '{expected.Value}' epxected", index)
        {

        }

        public TokenExpectedException(Token expected, Token token) : base($"Invlaid Token: '{token.Value}' detected, '{expected.Value}' expected", token.Index)
        {

        }

        public TokenExpectedException(string expected, Token token) : base($"Invalid Token: '{token.Value}' detected. {expected} expected", token.Index)
        {

        }
    }
}
