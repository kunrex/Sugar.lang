using System;

using Sugar.Language.Exceptions;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Interfaces.Invalid;

namespace Sugar.Language.Parsing.Nodes.Values.Invalid
{
    internal sealed class InvalidVariableNode : ParseNodeCollection, IInvalidNode<CompileException>
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Invalid; }

        private readonly CompileException exception;
        public CompileException Exception { get => exception; }

        private readonly ParseNodeCollection invalidNode;
        public ParseNodeCollection InvalidNode { get => invalidNode; }

        public InvalidVariableNode(CompileException _exception, ParseNodeCollection _invalidNode) : base(_invalidNode)
        {
            exception = _exception;
            invalidNode = _invalidNode;
        }
    }
}
