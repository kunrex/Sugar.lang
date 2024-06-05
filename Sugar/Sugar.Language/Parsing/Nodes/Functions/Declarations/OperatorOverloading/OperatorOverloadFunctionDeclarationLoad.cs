using System;

using Sugar.Language.Tokens.Operators;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Types;

using Sugar.Language.Parsing.Nodes.Describers;

using Sugar.Language.Parsing.Nodes.Values.Generics;

using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;

namespace Sugar.Language.Parsing.Nodes.Functions.Declarations.OperatorOverloading
{
    internal sealed class OperatorOverloadFunctionDeclarationNode : BaseFunctionDeclarationNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.OperatorOverload; }

        private readonly Operator oprator;
        public Operator Operator { get => oprator; }

        public OperatorOverloadFunctionDeclarationNode(DescriberNode _describer, TypeNode _returnType, FunctionParamatersNode _arguments, ParseNode _body, Operator _operator) : base(_describer, _returnType, _arguments, _body)
        {
            oprator = _operator;
        }

        public OperatorOverloadFunctionDeclarationNode(DescriberNode _describer, TypeNode _returnType, FunctionParamatersNode _arguments, ParseNode _body, Operator _operator, GenericDeclarationNode _generic) : base(_describer, _returnType, _arguments, _body, _generic)
        {
            oprator = _operator;
        }

        public override string ToString() => $"Operator Overload Declaration [Operator: {Operator.Value}]";
    }
}
