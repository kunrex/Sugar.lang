using System;
using System.Collections.Generic;

namespace Sugar.Language.Parsing.Nodes.Functions.Declarations
{
    internal abstract class UnnamedFunctionDeclarationNode : BaseFunctionDeclarationNode
    {
        public override Node Name { get => Type; }
        public override Node Arguments { get => Children[2]; }
        public override Node Body { get => Children[3]; }

        public UnnamedFunctionDeclarationNode(Node _describer, Node _returnType, Node _arguments, Node _body) : base(_describer, _returnType, null, _arguments, _body)
        {
            Children = new List<Node>() { _describer, _returnType, _arguments, _body };
        }
    }
}
