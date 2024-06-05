using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Types;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Parsing.Nodes.Describers;

using Sugar.Language.Parsing.Nodes.Values.Generics;

using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;

namespace Sugar.Language.Parsing.Nodes.Functions.Declarations
{
    internal sealed class ExtensionFunctionDeclarationNode : FunctionDeclarationNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.ExtensionDeclaration; }

        private readonly TypeNode extensionType;
        public TypeNode ExtensionType { get => extensionType; }

        public ExtensionFunctionDeclarationNode(DescriberNode _describer, TypeNode _returnType, IdentifierNode _name, FunctionParamatersNode _arguments, ParseNode _body, TypeNode _extensionType) : base(_describer, _returnType, _name, _arguments, _body)
        {
            extensionType = _extensionType;
            Add(extensionType);
        }

        public ExtensionFunctionDeclarationNode(DescriberNode _describer, TypeNode _returnType, IdentifierNode _name, FunctionParamatersNode _arguments, ParseNode _body, TypeNode _extensionType, GenericDeclarationNode _generic) : base(_describer, _returnType, _name, _arguments, _body, _generic)
        {
            extensionType = _extensionType;
            Add(extensionType);
        }

        public override string ToString() => $"Extension Function Declaration Node";
    }
}
