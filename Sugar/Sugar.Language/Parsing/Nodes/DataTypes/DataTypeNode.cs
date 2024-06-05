using System;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Values.Generics;

using Sugar.Language.Parsing.Nodes.Describers;

using Sugar.Language.Parsing.Nodes.Interfaces.Generic;
using Sugar.Language.Parsing.Nodes.Interfaces.Creation;

namespace Sugar.Language.Parsing.Nodes.DataTypes
{
    internal abstract class DataTypeNode : ParseNodeCollection, ICreationNode_Name, ICreationNode_Body, IGenericDeclaration
    {
        protected readonly DescriberNode describer;
        public DescriberNode Describer { get => describer; }

        protected readonly IdentifierNode name;
        public IdentifierNode Name { get => name; }

        protected readonly ParseNode body;
        public ParseNode Body { get => body; }

        protected readonly GenericDeclarationNode generic;
        public GenericDeclarationNode Generic { get => generic; }

        protected readonly InheritanceNode inheritance;
        public InheritanceNode Inheritance { get => inheritance; }

        public DataTypeNode(DescriberNode _describer, IdentifierNode _name, ParseNode _body) : base(_describer, _name, _body)
        {
            describer = _describer;

            name = _name;
            body = _body;

            generic = null;
            inheritance = null;
        }

        public DataTypeNode(DescriberNode _describer, IdentifierNode _name, ParseNode _body, GenericDeclarationNode _generic) : base(_describer, _name, _body, _generic)
        {
            describer = _describer;

            name = _name;
            body = _body;

            generic = _generic;
            inheritance = null;
        }

        public DataTypeNode(DescriberNode _describer, IdentifierNode _name, ParseNode _body, InheritanceNode _inheritance) : base(_describer, _name, _body, _inheritance)
        {
            describer = _describer;

            name = _name;
            body = _body;

            generic = null;
            inheritance = _inheritance;
        }

        public DataTypeNode(DescriberNode _describer, IdentifierNode _name, ParseNode _body, GenericDeclarationNode _generic, InheritanceNode _inheritance) : base(_describer, _name, _body, _generic, _inheritance)
        {
            describer = _describer;

            name = _name;
            body = _body;

            generic = _generic;
            inheritance = _inheritance;
        }

        public override ParseNode AddChild(ParseNode node) { return this; }
    }
}
