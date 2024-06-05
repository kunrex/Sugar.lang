using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Describers;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Values.Generics;

namespace Sugar.Language.Parsing.Nodes.DataTypes
{
    internal sealed class InterfaceNode : DataTypeNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Interface; }
 
        public InterfaceNode(DescriberNode _describer, IdentifierNode _name, ParseNode _body) : base(_describer, _name, _body)
        {

        }

        public InterfaceNode(DescriberNode _describer, IdentifierNode _name, ParseNode _body, GenericDeclarationNode _generic) : base(_describer, _name, _body, _generic)
        {

        }

        public InterfaceNode(DescriberNode _describer, IdentifierNode _name, ParseNode _body, InheritanceNode _inheritance) : base(_describer, _name, _body, _inheritance)
        {

        }

        public InterfaceNode(DescriberNode _describer, IdentifierNode _name, ParseNode _body, GenericDeclarationNode _generic, InheritanceNode _inheritance) : base(_describer, _name, _body, _generic, _inheritance)
        {

        }

        public override string ToString() => $"Interface Node";
    }
}
