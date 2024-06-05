using System;

using Sugar.Language.Exceptions;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Types.Enums;

using Sugar.Language.Parsing.Nodes.Interfaces.Invalid;

namespace Sugar.Language.Parsing.Nodes.Types.Invalid
{
    internal sealed class InvalidTypeNode : TypeNode, IInvalidNode<CompileException>
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Invalid; }

        public override TypeNodeEnum Type { get => throw new NotImplementedException(); }

        private readonly CompileException exception;
        public CompileException Exception { get => exception; }

        private readonly ParseNodeCollection invalidNode;
        public ParseNodeCollection InvalidNode { get => invalidNode; }

        public InvalidTypeNode(CompileException _exception, ParseNodeCollection _invalidNode) : base(_invalidNode)
        {
            exception = _exception;
            invalidNode = _invalidNode;
        }
    }
}
