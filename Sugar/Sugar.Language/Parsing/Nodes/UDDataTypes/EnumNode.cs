using System;

using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.UDDataTypes.Enums;

namespace Sugar.Language.Parsing.Nodes.UDDataTypes
{
    internal sealed class EnumNode : UDDataTypeNode
    {
        public override NodeType NodeType => NodeType.Enum;
        public override UDDataType DataType => UDDataType.Enum;

        public EnumNode(Node _describer, Node _name, Node _body) : base(_describer, _name, _body)
        {

        }

        public override string ToString() => $"Enum Node";
    }
}
