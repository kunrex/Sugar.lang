using System;

using Sugar.Language.Exceptions.Parsing;

using Sugar.Language.Tokens;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Interfaces.Invalid;

namespace Sugar.Language.Parsing.Nodes
{
    internal sealed class InvalidStatementNode : ParseNodeCollection, IInvalidTokenCollectionNode<InvalidStatementException>
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Invalid; }

        private readonly InvalidStatementException exception;
        public InvalidStatementException Exception { get => exception; }

        private readonly TokenCollection collecton;
        public TokenCollection Collection { get => collecton; }

        public InvalidStatementNode(InvalidStatementException _exception, TokenCollection _collection)
        {
            exception = _exception;
            collecton = _collection;
        }
    }
}
