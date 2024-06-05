using System;

using Sugar.Language.Exceptions.Parsing;

using Sugar.Language.Tokens;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Interfaces.Invalid;

namespace Sugar.Language.Parsing.Nodes.Values.Invalid
{
    internal sealed class InvalidIdentifierNode : IdentifierNode, IInvalidTokenNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Invalid; }

        private readonly Token token;
        public Token InvalidToken { get => token; }

        private readonly InvalidTokenException exception;
        public InvalidTokenException Exception { get => exception; }

        public InvalidIdentifierNode(Token _token, InvalidTokenException _exception) : base(null)
        {
            token = _token;
            exception = _exception;
        }

        public override string ToString() => $"[INVALID] Identifier Node [Value: {token.Value}]";
    }
}
