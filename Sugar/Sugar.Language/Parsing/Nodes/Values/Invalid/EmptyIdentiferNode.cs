using System;
using Sugar.Language.Exceptions.Parsing;
using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Interfaces.Invalid;
using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Parsing.Nodes.Values.Invalid
{
    internal sealed class EmptyEntityNode : ParseNodeCollection, IExpectedTokenNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Invalid; }

        private readonly TokenExpectedException exception;
        public TokenExpectedException Exception { get => exception; }

        private readonly TokenType expected;
        public TokenType ExpectedToken { get => expected; }

        public EmptyEntityNode(TokenExpectedException _exception, TokenType _expected) : base()
        {
            exception = _exception;

            expected = _expected;
        }
    }
}
