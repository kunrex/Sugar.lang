using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Describers;

using Sugar.Language.Parsing.Nodes.Values.Generics;

using Sugar.Language.Parsing.Nodes.Interfaces.Generic;
using Sugar.Language.Parsing.Nodes.Interfaces.Creation;

using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;

namespace Sugar.Language.Parsing.Nodes.Declarations.Delegates
{
    internal class DelegateNode : ParseNodeCollection, ICreationNode_Body, IGenericDeclaration
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Delegate; }

        protected readonly FunctionParamatersNode arguments;
        public FunctionParamatersNode Arguments { get => arguments; }

        protected readonly ParseNode body;
        public ParseNode Body { get => body; }

        protected readonly GenericDeclarationNode generic;
        public GenericDeclarationNode Generic { get => generic; }

        public DelegateNode(DescriberNode _describer, FunctionParamatersNode _arguments, ParseNode _body) : base(_describer, _arguments, _body)
        {
            arguments = _arguments;
            body = _body;

            generic = null;
        }

        public DelegateNode(DescriberNode _describer, FunctionParamatersNode _arguments, ParseNode _body, GenericDeclarationNode _generic) : base(_describer, _arguments, _body, _generic)
        {
            arguments = _arguments;
            body = _body;

            generic = _generic;
        }
    }
}
