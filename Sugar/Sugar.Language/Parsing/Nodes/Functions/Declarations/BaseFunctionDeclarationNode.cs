using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Interfaces;
using Sugar.Language.Parsing.Nodes.Interfaces.Creation;

namespace Sugar.Language.Parsing.Nodes.Functions.Declarations
{
    internal abstract class BaseFunctionDeclarationNode : Node, ICreationNode_Type, ICreationNode_Name, ICreationNode_Body, IGenericNode
    {
        public override NodeType NodeType => NodeType.FunctionDeclaration;

        public Node Describer { get => Children[0]; }
        public Node Type { get => Children[1]; }

        public virtual Node Name  { get => Children[2]; }
        public virtual Node Arguments { get => Children[3]; }
        public virtual Node Body { get => Children[4]; }

        private int genericIndex = -1;
        public Node Generic { get => genericIndex == -1 ? null : Children[genericIndex]; }

        public BaseFunctionDeclarationNode(Node _describer, Node _returnType, Node _name, Node _arguments, Node _body)
        {
            Children = new List<Node>() { _describer, _returnType, _name, _arguments, _body };
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
