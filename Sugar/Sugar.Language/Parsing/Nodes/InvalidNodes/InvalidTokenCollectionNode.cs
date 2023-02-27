using System;
using System.Collections.Generic;

using Sugar.Language.Tokens;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Exceptions;

namespace Sugar.Language.Parsing.Nodes.InvalidNodes
{
    internal sealed class InvalidTokenCollectionNode : Node, IInvalidNode
    {
        public override NodeType NodeType { get => NodeType.InvalidTokenCollection; }

        private readonly List<Token> tokens;
        public List<Token> Tokens { get => tokens; }

        private readonly CompileException exception;
        public CompileException Exception { get => exception; }

        public InvalidTokenCollectionNode(CompileException _exception, List<Token> _tokens)
        {
            tokens = _tokens;
            exception = _exception;
        }

        public override string ToString() => $"Invalid Token Collection";
    }
}
