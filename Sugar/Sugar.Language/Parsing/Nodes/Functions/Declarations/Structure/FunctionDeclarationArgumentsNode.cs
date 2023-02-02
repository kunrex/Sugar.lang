using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure
{
    internal sealed class FunctionDeclarationArgumentsNode : Node
    {
        public override NodeType NodeType => NodeType.MethodDeclaration;

        public FunctionDeclarationArgumentsNode(List<Node> _arguments)
        {
            Children = _arguments;
        }

        public override string ToString() => $"Function Declaration Arguments Node";
    }
}
