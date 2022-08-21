using System;
using System.Collections.Generic;
using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Functions.Declarations
{
    internal sealed class ConstructorDeclarationNode : UnnamedFunctionDeclarationNode
    {
        public Node ParentOverrideNode { get => Children.Count == 4 ? null : Children[4]; }

        public override NodeType NodeType => NodeType.ConstructorDeclaration;

        public ConstructorDeclarationNode(Node _describer, Node _returnType, Node _arguments, Node _body) : base(_describer, _returnType, _arguments, _body)
        {
           
        }

        public ConstructorDeclarationNode(Node _describer, Node _returnType, Node _arguments, Node _body, Node _parent) : base(_describer, _returnType, _arguments, _body)
        {
            Children = new List<Node>() { _describer, _returnType, _arguments, _body, _parent };
        }

        public override string ToString() => $"Constructor Declaration Node";
    }
}
