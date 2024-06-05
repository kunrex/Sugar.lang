using System;

using Sugar.Language.Exceptions;

using Sugar.Language.Tokens;

namespace Sugar.Language.Parsing.Nodes.Interfaces.Invalid
{
    internal interface IInvalidTokenCollectionNode<Exception> : IInvalidNode<Exception> where Exception : CompileException
    {
        public TokenCollection Collection { get; }
    }
}
