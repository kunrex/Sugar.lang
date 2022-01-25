using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Interfaces;
using Sugar.Language.Parsing.Nodes.Functions.Calling.Structure;

namespace Sugar.Language.Parsing.Nodes.Functions.Calling
{
    internal abstract class BaseFunctionCallNode : Node, IGenericNode
    {
        public Node Value { get => Children[0]; }
        public Node Arguments { get => Children[1]; }
        public override NodeType NodeType => NodeType.FunctionCall;

        private int genericIndex = -1;
        public Node Generic { get => genericIndex == -1 ? null : Children[genericIndex]; }

        public BaseFunctionCallNode(FunctionNameCallNode _value, FunctionCallArgumentsNode _arguments)
        {
            Children = new List<Node>() { _value, _arguments };
        }

        public override Node AddChild(Node _node)
        {
            Children.Add(_node);
            if (_node.NodeType == NodeType.Generic)
                genericIndex = Children.Count - 1;

            return this;
        }
    }
}
