using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Types;

using Sugar.Language.Parsing.Nodes.Describers;

using Sugar.Language.Parsing.Nodes.Values.Generics;

using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;

namespace Sugar.Language.Parsing.Nodes.Functions.Declarations.OperatorOverloading
{
    internal sealed class ImplicitCastDeclarationNode : BaseFunctionDeclarationNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.ImplicitDeclaration; }

        public ImplicitCastDeclarationNode(DescriberNode _describer, TypeNode _returnType, FunctionParamatersNode _arguments, ParseNode _body) : base(_describer, _returnType, _arguments, _body)
        {

        }

        public ImplicitCastDeclarationNode(DescriberNode _describer, TypeNode _returnType, FunctionParamatersNode _arguments, ParseNode _body, GenericDeclarationNode _generic) : base(_describer, _returnType, _arguments, _body, _generic)
        {

        }

        public override string ToString() => $"Implicit Cast Declaration Node";
    }
}
