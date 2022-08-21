using System;

using Sugar.Language.Tokens.Operators;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.Functions.Declarations.OperatorOverloading
{
    internal sealed class OperatorOverloadFunctionDeclarationNode : UnnamedFunctionDeclarationNode
    {
        public Operator Operator { get; private set; }

        public override NodeType NodeType { get => NodeType.OperatorOverload; }

        public OperatorOverloadFunctionDeclarationNode(Node _describer, Node _returnType, Node _arguments, Node _body, Operator _operator) : base(_describer, _returnType, _arguments, _body)
        {
            Operator = _operator;
        }

        public override string ToString() => $"Operator Overload Declaration [Operator: {Operator.Value}]";
    }
}
