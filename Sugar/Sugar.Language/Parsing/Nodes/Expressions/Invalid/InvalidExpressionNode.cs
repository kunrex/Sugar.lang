using System;

using Sugar.Language.Exceptions;

using Sugar.Language.Tokens;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Interfaces.Invalid;

namespace Sugar.Language.Parsing.Nodes.Expressions.Invalid
{
    internal sealed class InvalidExpressionNode : ExpressionNode, IInvalidExpressionNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Invalid; }

        private readonly CompileException exception;
        public CompileException Exception { get => exception; }

        private readonly TokenCollection tokens;
        public TokenCollection InvalidTokenCollection { get => tokens; }

        public Token InvalidToken { get => tokens[tokens.Length - 1]; }

        public InvalidExpressionNode(CompileException _exception, TokenCollection _tokens)
        {
            exception = _exception;
            tokens = _tokens;
        }
    }
}
