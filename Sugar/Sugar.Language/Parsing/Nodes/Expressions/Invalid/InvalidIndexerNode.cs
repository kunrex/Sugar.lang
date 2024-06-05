using System;

using Sugar.Language.Exceptions;

using Sugar.Language.Tokens;

using Sugar.Language.Parsing.Nodes.Interfaces.Invalid;

using Sugar.Language.Parsing.Nodes.Expressions.Associative;

namespace Sugar.Language.Parsing.Nodes.Expressions.Invalid
{
    internal sealed class InvalidIndexerNode : IndexerExpression, IInvalidTokenCollectionNode<CompileException>
    {
        private readonly CompileException exception;
        public CompileException Exception { get => exception; }

        private readonly TokenCollection tokens;
        public TokenCollection Collection { get => tokens; }

        public InvalidIndexerNode(CompileException _exception, TokenCollection _tokens) : base(null, null)
        {
            exception = _exception;
            tokens = _tokens;
        }

        public override string ToString() => $"[INVALID] Indexer Node";
    }
}
