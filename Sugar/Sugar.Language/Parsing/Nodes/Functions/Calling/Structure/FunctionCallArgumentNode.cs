using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Functions.Calling.Structure
{
    internal sealed class FunctionCallArgumentNode : Node
    {
        public Node Value { get => Children[ChildCount - 1]; }
        public Node Describer { get => ChildCount == 2 ? null : Children[0]; }

        public override NodeType NodeType => NodeType.ArgumentCall;

        public FunctionCallArgumentNode(Node _value)
        {
            Children = new List<Node> { _value };
        }

        public FunctionCallArgumentNode(Node _desrciber, Node _value)
        {
            Children = new List<Node> { _desrciber, _value };
        }

        public override string ToString() => $"Function Call Argument Node";
    }
}
