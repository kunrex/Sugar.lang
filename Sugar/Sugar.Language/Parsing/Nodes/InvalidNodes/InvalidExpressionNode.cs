using System;
using System.Collections.Generic;

using Sugar.Language.Exceptions;

using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Expressions;

namespace Sugar.Language.Parsing.Nodes.InvalidNodes
{
    internal sealed class InvalidExpressionNode : ExpressionNode, IInvalidNode
    {
        public override NodeType NodeType { get => NodeType.Invalid; }
        public InvalidNodeType InvalidNodeType { get => InvalidNodeType.InvalidExpression; }

        private readonly CompileException exception;
        public CompileException Exception { get => exception; }

        public Node Stack { get => Children[0]; }
        public Node EvaluatedOutput { get => Children[1]; }
        public Node InvalidTokenCollection { get => Children[2]; }

        public InvalidExpressionNode(CompileException _exception)
        {
            exception = _exception;

            var empty = new EmptyNode();
            Children = new List<Node>() { empty, empty, empty };
        }

        public InvalidExpressionNode(Node stack, Node output, Node invalid)
        {
            exception = ((IInvalidNode)invalid).Exception;
            Children = new List<Node>() { stack , output, invalid };
        }

        public override string ToString() => $"Invalid Expression Node";
    }
}
