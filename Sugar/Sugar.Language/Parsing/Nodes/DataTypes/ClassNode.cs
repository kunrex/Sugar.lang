using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Describers;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Values.Generics;

namespace Sugar.Language.Parsing.Nodes.DataTypes
{
    internal sealed class ClassNode : DataTypeNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Class; }

        public ClassNode(DescriberNode _describer, IdentifierNode _name, ParseNode _body) : base(_describer, _name, _body)
        {

        }

        public ClassNode(DescriberNode _describer, IdentifierNode _name, ParseNode _body, GenericDeclarationNode _generic) : base(_describer, _name, _body, _generic)
        {

        }

        public ClassNode(DescriberNode _describer, IdentifierNode _name, ParseNode _body, InheritanceNode _inheritance) : base(_describer, _name, _body, _inheritance)
        {

        }

        public ClassNode(DescriberNode _describer, IdentifierNode _name, ParseNode _body, GenericDeclarationNode _generic, InheritanceNode _inheritance) : base(_describer, _name, _body, _generic, _inheritance)
        {

        }

        public override string ToString() => $"Class Node";
    }
}
