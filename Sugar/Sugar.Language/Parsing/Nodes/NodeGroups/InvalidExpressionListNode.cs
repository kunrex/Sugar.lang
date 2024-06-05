using System;

using Sugar.Language.Exceptions;

using Sugar.Language.Tokens;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Interfaces.Invalid;

namespace Sugar.Language.Parsing.Nodes.NodeGroups
{
    internal class InvalidExpressionListNode : ExpressionListNode, IInvalidNode<CompileException>
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Invalid; }

        private readonly CompileException exception;
        public CompileException Exception { get => exception; }

        private readonly TokenCollection tokens;
        public TokenCollection Tokens { get => tokens; }

        public InvalidExpressionListNode(CompileException _exception, TokenCollection _tokens) : base()
        {
            exception = _exception;

            tokens = _tokens;
        }
    }
}
