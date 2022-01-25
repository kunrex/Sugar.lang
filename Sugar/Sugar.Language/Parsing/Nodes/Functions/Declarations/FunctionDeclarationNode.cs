using System;

namespace Sugar.Language.Parsing.Nodes.Functions.Declarations
{
    internal sealed class FunctionDeclarationNode : BaseFunctionDeclarationNode
    {
        public FunctionDeclarationNode(Node _describer, Node _returnType, Node _name, Node _arguments, Node _body) : base(_describer, _returnType, _name, _arguments, _body)
        {

        }

        public override string ToString() => $"Function Declaration Node";
    }
}
