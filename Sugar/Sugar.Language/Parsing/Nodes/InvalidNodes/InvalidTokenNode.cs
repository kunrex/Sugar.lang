using System;

using Sugar.Language.Tokens;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Exceptions;

namespace Sugar.Language.Parsing.Nodes.InvalidNodes
{
    internal sealed class InvalidTokenNode : Node, IInvalidNode
    {
        public override NodeType NodeType { get => NodeType.Invalid; }

        private readonly CompileException exception;
        public CompileException Exception { get => exception; }

        private readonly Token invalid;
        public Token Invalid { get => invalid; }

        public InvalidNodeType InvalidNodeType { get => InvalidNodeType.ExpectedToken; }

        public InvalidTokenNode(CompileException _exception, Token _invalid)
        {
            invalid = _invalid;
            exception = _exception;
        }

        public override string ToString() => $"Invlaid Token Node [{invalid}]";
    }
}
