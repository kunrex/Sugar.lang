using System;

using Sugar.Language.Parsing.Nodes.Types;

using Sugar.Language.Parsing.Nodes.Values.Generics;

using Sugar.Language.Parsing.Nodes.Describers;

using Sugar.Language.Parsing.Nodes.Interfaces.Generic;
using Sugar.Language.Parsing.Nodes.Interfaces.Creation;

using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;

namespace Sugar.Language.Parsing.Nodes.Functions.Declarations
{
    internal abstract class BaseFunctionDeclarationNode : ParseNodeCollection, ICreationNode_Type, ICreationNode_Body, IGenericDeclaration
    {
        protected readonly DescriberNode describer;
        public DescriberNode Describer { get => describer; }

        protected readonly TypeNode type;
        public TypeNode Type { get => type; }

        protected readonly FunctionParamatersNode arguments;
        public FunctionParamatersNode Arguments { get => arguments; }

        protected readonly ParseNode body;
        public ParseNode Body { get => body; }

        protected readonly GenericDeclarationNode generic;
        public GenericDeclarationNode Generic { get => generic; }

        public BaseFunctionDeclarationNode(DescriberNode _describer, TypeNode _type, FunctionParamatersNode _arguments, ParseNode _body) : base(_describer, _type, _arguments, _body)
        {
            describer = _describer;

            type = _type;
            arguments = _arguments;

            body = _body;
            generic = null;
        }

        public BaseFunctionDeclarationNode(DescriberNode _describer, TypeNode _type, FunctionParamatersNode _arguments, ParseNode _body, GenericDeclarationNode _generic) : base(_describer, _type, _arguments, _body, _generic)
        {
            describer = _describer;

            type = _type;
            arguments = _arguments;

            body = _body;
            generic = _generic;
        }

        public override ParseNode AddChild(ParseNode node) { return this; }
    }
}
