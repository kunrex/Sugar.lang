using System;

using Sugar.Language.Exceptions;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Interfaces.Invalid;

namespace Sugar.Language.Parsing.Nodes.Expressions.Invalid
{
    internal sealed class EmptyExpressionNode : ExpressionNode, IInvalidNode<CompileException>
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Invalid; }

        private readonly CompileException exception;
        public CompileException Exception { get => exception; }

        public EmptyExpressionNode(CompileException _exception)
        {
            exception = _exception;
        }
    }
}
