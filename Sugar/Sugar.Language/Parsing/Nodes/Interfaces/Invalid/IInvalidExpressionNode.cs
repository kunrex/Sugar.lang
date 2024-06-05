using System;

using Sugar.Language.Exceptions;

using Sugar.Language.Tokens;

namespace Sugar.Language.Parsing.Nodes.Interfaces.Invalid
{
    internal interface IInvalidExpressionNode : IInvalidNode<CompileException>
    {
        public TokenCollection InvalidTokenCollection { get; }

        public Token InvalidToken { get; }
    }
}
