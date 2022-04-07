using System;
using System.Collections.Generic;
using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure
{
    internal sealed class ParentFunctionArgumentNode : Node
    {
        public Node Arguments { get => Children[0]; }

        public override NodeType NodeType { get => NodeType.Parent; }

        public ParentFunctionArgumentNode(Node _arguments)
        {
            Children = new List<Node>() { _arguments };
        }

        public override string ToString() => $"Parent Constructor Override Node";
    }
}
