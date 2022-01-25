using System;
using System.Collections.Generic;

namespace Sugar.Language.Parsing.Nodes.Functions.Declarations
{
    internal sealed class ExtensionFunctionDeclarationNode : BaseFunctionDeclarationNode
    {
        public ExtensionFunctionDeclarationNode(Node _describer, Node _returnType, Node _name, Node _arguments, Node _body, Node _extensionType) : base(_describer, _returnType, _name, _arguments, _body)
        {
            Children = new List<Node>() { _describer, _returnType, _name, _arguments, _body, _extensionType };
        }

        public override string ToString() => $"Extension Function Declaration Node";
    }
}
