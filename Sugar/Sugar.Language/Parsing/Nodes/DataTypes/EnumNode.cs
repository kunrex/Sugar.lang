using System;

using Sugar.Language.Parsing.Nodes.Enums;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Parsing.Nodes.Describers;

namespace Sugar.Language.Parsing.Nodes.DataTypes
{
    internal sealed class EnumNode : DataTypeNode
    {
        public override ParseNodeType NodeType { get => ParseNodeType.Enum; }

        public EnumNode(DescriberNode _describer, IdentifierNode _name, ParseNode _body) : base(_describer, _name, _body)
        {

        }

        public EnumNode(DescriberNode _describer, IdentifierNode _name, ParseNode _body, InheritanceNode _inheritance) : base(_describer, _name, _body, _inheritance)
        {

        }


        public override string ToString() => $"Enum Node";
    }
}
