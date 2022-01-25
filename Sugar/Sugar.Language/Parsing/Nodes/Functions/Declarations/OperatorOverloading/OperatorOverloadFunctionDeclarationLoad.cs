using System;

using Sugar.Language.Tokens.Operators;

namespace Sugar.Language.Parsing.Nodes.Functions.Declarations.OperatorOverloading
{
    internal sealed class OperatorOverloadFunctionDeclarationLoad : UnnamedFunctionDeclarationNode
    {
        public Operator Operator { get; private set; }

        public OperatorOverloadFunctionDeclarationLoad(Node _describer, Node _returnType, Node _arguments, Node _body, Operator _operator) : base(_describer, _returnType, _arguments, _body)
        {
            Operator = _operator;
        }

        public override string ToString() => $"Operator Overload Declaration [Operator: {Operator.Value}]";
    }
}
