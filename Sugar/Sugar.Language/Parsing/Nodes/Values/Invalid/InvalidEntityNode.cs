using System;

using Sugar.Language.Exceptions;

using Sugar.Language.Tokens;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Interfaces.Invalid;

namespace Sugar.Language.Parsing.Nodes.Values.Invalid
{
    internal sealed class InvalidEntityNode : ParseNodeCollection, IInvalidTokenCollectionNode<CompileException>
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Invalid; }

        private readonly CompileException exception;
        public CompileException Exception { get => exception; }

        private readonly TokenCollection collecton;
        public TokenCollection Collection { get => collecton; }

        public InvalidEntityNode(CompileException _exception, TokenCollection _collection)
        {
            exception = _exception;
            collecton = _collection;
        }
    }
}
