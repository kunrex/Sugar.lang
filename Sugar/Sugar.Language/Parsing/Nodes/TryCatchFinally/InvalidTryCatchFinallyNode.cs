using System;

using Sugar.Language.Exceptions;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Interfaces.Invalid;

using Sugar.Language.Parsing.Nodes.TryCatchFinally.Blocks;

namespace Sugar.Language.Parsing.Nodes.TryCatchFinally
{
    internal sealed class InvalidTryCatchFinallyNode : TryCatchFinallyBlockNode, IInvalidNode<CompileException>
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Invalid; }

        private readonly CompileException exception;
        public CompileException Exception { get => exception; }

        public InvalidTryCatchFinallyNode(CompileException _exception, TryBlockNode _try) : base(_try)
        {
            exception = _exception;
        }

        public override string ToString() => $"[INVALID] Try Catch Finally Node";
    }
}
