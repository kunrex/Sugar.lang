using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Interfaces;
using Sugar.Language.Parsing.Nodes.UDDataTypes.Enums;
using Sugar.Language.Parsing.Nodes.Interfaces.Creation;

namespace Sugar.Language.Parsing.Nodes.UDDataTypes
{
    internal abstract class UDDataTypeNode : Node, ICreationNode_Name, ICreationNode_Body, IGenericNode
    {
        public abstract UDDataType DataType { get; }

        public Node Describer { get => Children[0]; }

        public Node Name { get => Children[1]; }
        public Node Body { get => Children[2]; }

        private int genericIndex = -1;
        public Node Generic { get => genericIndex == -1 ? null : Children[genericIndex]; }

        private int inheritsIndex = -1;
        public Node Inherits { get => inheritsIndex == -1 ? null : Children[inheritsIndex]; }

        public UDDataTypeNode(Node _describer, Node _name, Node _body)
        {
            Children = new List<Node>() { _describer, _name, _body };
        }

        public override Node AddChild(Node _node)
        {
            Children.Add(_node);
            if (_node.NodeType == NodeType.Generic)
                genericIndex = Children.Count - 1;
            else if (_node.NodeType == NodeType.Inheritance)
                inheritsIndex = Children.Count - 1;

            return this;
        }
    }
}
