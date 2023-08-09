using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Exceptions;

namespace Sugar.Language.Parsing.Nodes.InvalidNodes
{
    internal sealed class TokenExpectedNode : Node, IInvalidNode
    {
        public override NodeType NodeType { get => NodeType.Invalid; }

        private readonly CompileException exception;
        public CompileException Exception { get => exception; }

        public InvalidNodeType InvalidNodeType { get => InvalidNodeType.ExpectedToken; }

        public TokenExpectedNode(CompileException _exception)
        {
            exception = _exception;
        }

        public override string ToString() => $"Token Expected Node";
    }
}
