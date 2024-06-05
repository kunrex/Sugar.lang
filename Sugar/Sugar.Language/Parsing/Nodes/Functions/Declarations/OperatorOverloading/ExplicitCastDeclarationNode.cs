using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Types;

using Sugar.Language.Parsing.Nodes.Describers;

using Sugar.Language.Parsing.Nodes.Values.Generics;

using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;

namespace Sugar.Language.Parsing.Nodes.Functions.Declarations.OperatorOverloading
{
    internal sealed class ExplicitCastDeclarationNode : BaseFunctionDeclarationNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.ExplicitDeclaration; }

        public ExplicitCastDeclarationNode(DescriberNode _describer, TypeNode _returnType, FunctionParamatersNode _arguments, ParseNode _body) : base(_describer, _returnType, _arguments, _body)
        {

        }

        public ExplicitCastDeclarationNode(DescriberNode _describer, TypeNode _returnType, FunctionParamatersNode _arguments, ParseNode _body, GenericDeclarationNode _generic) : base(_describer, _returnType, _arguments, _body, _generic)
        {

        }

        public override string ToString() => $"Explicit Cast Declaration Node";
    }
}

