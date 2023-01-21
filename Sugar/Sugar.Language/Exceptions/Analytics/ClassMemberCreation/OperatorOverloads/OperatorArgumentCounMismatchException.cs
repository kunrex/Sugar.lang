using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Exceptions.Analytics.ClassMemberCreation.OperatorOverloads
{
    internal class OperatorArgumentCounMismatchException : CompileException
    {
        public OperatorArgumentCounMismatchException(TokenType type) : base($"A {type} operator must have at least {(type == TokenType.UnaryOperator ? 1 : 2)}", 0)
        {

        }
    }
}
