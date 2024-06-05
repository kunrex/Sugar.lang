using System;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Statements
{
    internal sealed class ThrowExceptionNode : StatementNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.ThrowException; }

        private readonly ParseNodeCollection exception;
        public ParseNodeCollection Exception { get => exception; }

        public ThrowExceptionNode(ParseNodeCollection _exception) : base(_exception)
        {
            exception = _exception;
        }

        public override string ToString() => $"Throw Excpetion Node";
    }
}
