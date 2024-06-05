using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Types;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Parsing.Nodes.Describers;

using Sugar.Language.Parsing.Nodes.Values.Generics;

using Sugar.Language.Parsing.Nodes.Interfaces.Creation;

using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;

namespace Sugar.Language.Parsing.Nodes.Functions.Declarations
{
    internal class FunctionDeclarationNode : BaseFunctionDeclarationNode, ICreationNode_Name
    {
        public override ParseNodeType NodeType { get => ParseNodeType.FunctionDeclaration; }

        protected readonly IdentifierNode name;
        public IdentifierNode Name { get => name; }

        public FunctionDeclarationNode(DescriberNode _describer, TypeNode _returnType, IdentifierNode _name, FunctionParamatersNode _arguments, ParseNode _body) : base(_describer, _returnType, _arguments, _body)
        {
            name = _name;
            Add(name);
        }

        public FunctionDeclarationNode(DescriberNode _describer, TypeNode _returnType, IdentifierNode _name, FunctionParamatersNode _arguments, ParseNode _body, GenericDeclarationNode _generic) : base(_describer, _returnType, _arguments, _body, _generic)
        {
            name = _name;
            Add(name);
        }

        public override string ToString() => $"Function Declaration Node";
    }
}
