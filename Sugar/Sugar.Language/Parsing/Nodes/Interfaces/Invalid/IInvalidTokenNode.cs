using System;

using Sugar.Language.Exceptions.Parsing;

using Sugar.Language.Tokens;

namespace Sugar.Language.Parsing.Nodes.Interfaces.Invalid
{
    internal interface IInvalidTokenNode : IInvalidNode<InvalidTokenException>
    {
        public Token InvalidToken { get; }
    }
}
