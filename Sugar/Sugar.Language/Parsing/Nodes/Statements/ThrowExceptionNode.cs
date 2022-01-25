using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Statements
{
    internal sealed class ThrowExceptionNode : StatementNode
    {
        public Node Exception { get => Children[0]; }
        public override NodeType NodeType => NodeType.ThrowException;

        public ThrowExceptionNode(Node _exception)
        {
            Children = new List<Node>() { _exception };
        }

        public override string ToString() => $"Throw Excpetion Node";
    }
}
