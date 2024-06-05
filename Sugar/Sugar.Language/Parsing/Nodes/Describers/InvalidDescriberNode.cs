using System;

using Sugar.Language.Exceptions;

using Sugar.Language.Tokens;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Interfaces.Invalid;

namespace Sugar.Language.Parsing.Nodes.Describers
{
    internal sealed class InvalidDescriberNode : DescriberKeywordNode, IInvalidTokenCollectionNode<CompileException>
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Invalid; }

        private readonly TokenCollection tokens;
        public TokenCollection Collection { get => tokens; }

        private readonly CompileException exception;
        public CompileException Exception { get => exception; }

        public InvalidDescriberNode(TokenCollection _tokens, CompileException _exception) : base(null)
        {
            tokens = _tokens;
            exception = _exception;
        }

        public override string ToString() => $"[INVALID] Describer Keyword Node";
    }
}
