using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Describers;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Values.Generics;

namespace Sugar.Language.Parsing.Nodes.DataTypes
{
    internal sealed class StructNode : DataTypeNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Struct; }

        public StructNode(DescriberNode _describer, IdentifierNode _name, ParseNode _body) : base(_describer, _name, _body)
        {

        }

        public StructNode(DescriberNode _describer, IdentifierNode _name, ParseNode _body, GenericDeclarationNode _generic) : base(_describer, _name, _body, _generic)
        {

        }

        public override string ToString() => $"Struct Node";
    }
}
