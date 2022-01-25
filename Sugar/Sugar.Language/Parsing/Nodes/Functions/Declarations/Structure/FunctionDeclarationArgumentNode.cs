using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure
{
    internal sealed class FunctionDeclarationArgumentNode : Node
    {
        public override NodeType NodeType => NodeType.ArgumentDeclaration;

        public Node Describer { get => Children[0]; }

        public Node Type { get => Children[1]; }
        public Node Name { get => Children[2]; }

        public Node DefaultValue { get => ChildCount == 3 ? null : Children[3]; }

        public FunctionDeclarationArgumentNode(Node _describer, Node _type, Node _name)
        {
            Children = new List<Node>() { _describer, _type, _name };
        }

        public FunctionDeclarationArgumentNode(Node _describer, Node _type, Node _name, Node _defaultValue)
        {
            Children = new List<Node>() { _describer, _type, _name, _defaultValue };
        }

        public override string ToString() => $"Function Declaration Argument Node";
    }
}
