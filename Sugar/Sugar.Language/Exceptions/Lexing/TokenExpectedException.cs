using System;

using Sugar.Language.Tokens;
using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Exceptions.Lexing
{
    internal sealed class TokenExpectedException : CompileException
    {
        public TokenExpectedException(TokenType expected, int index) : base($"Invlaid Token detected, {expected} epxected", index)
        {

        }

        public TokenExpectedException(TokenType expected, Token token, int index) : base($"Invlaid Token: '{token.Value}' detected, {expected} epxected", index)
        {

        }

        public TokenExpectedException(Token expected,int index) : base($"Invlaid Token detected, '{expected.Value}' epxected", index)
        {

        }

        public TokenExpectedException(Token expected, Token token, int index) : base($"Invlaid Token: '{token.Value}' detected, '{expected.Value}' epxected", index)
        {

        }

        public TokenExpectedException(Token token, string expected, int index) : base($"Invalid Token: '{token.Value}' detected. {expected} expected", index)
        {

        }
    }
}
