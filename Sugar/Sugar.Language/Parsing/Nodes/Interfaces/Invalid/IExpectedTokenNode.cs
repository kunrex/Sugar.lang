using System;

using Sugar.Language.Exceptions.Parsing;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Parsing.Nodes.Interfaces.Invalid
{
    internal interface IExpectedTokenNode : IInvalidNode<TokenExpectedException>
    {
        public TokenType ExpectedToken { get; }
    }
}
