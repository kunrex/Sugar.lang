using System;

namespace Sugar.Language.Parsing.Nodes.Functions.Declarations
{
    internal sealed class ConstructorDeclarationNode : UnnamedFunctionDeclarationNode
    {
        public ConstructorDeclarationNode(Node _describer, Node _returnType, Node _arguments, Node _body) : base(_describer, _returnType, _arguments, _body)
        {
           
        }

        public override string ToString() => $"Constructor Declaration Node";
    }
}
