using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Exceptions;

namespace Sugar.Language.Parsing.Nodes.InvalidNodes
{
    internal sealed class InvalidEntityNode : Node, IInvalidNode
    {
        public override NodeType NodeType { get => NodeType.Invalid; }
        public InvalidNodeType InvalidNodeType { get => InvalidNodeType.InvalidEntity; }

        private readonly CompileException exception;
        public CompileException Exception { get => exception; }

        public Node Invalid { get => Children[0]; }

        private readonly NodeType expectedEntityType;
        public NodeType ExpectedEntityType { get => expectedEntityType; }

        public NodeType InvalidType { get => Children[0].NodeType; }

        public InvalidEntityNode(CompileException _exception, Node _invalid, NodeType _expected)
        {
            exception = _exception;
            expectedEntityType = _expected;

            Children = new List<Node>() { _invalid };
        }

        public override string ToString() => $"Token Expected Node";
    }
}
