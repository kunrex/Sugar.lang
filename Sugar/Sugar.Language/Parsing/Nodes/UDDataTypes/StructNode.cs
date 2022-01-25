using System;

using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.UDDataTypes.Enums;

namespace Sugar.Language.Parsing.Nodes.UDDataTypes
{
    internal sealed class StructNode : UDDataTypeNode
    {
        public override NodeType NodeType => NodeType.Struct;
        public override UDDataType DataType => UDDataType.Struct;

        public StructNode(Node _describer, Node _name, Node _body) : base(_describer, _name, _body)
        {

        }

        public override string ToString() => $"Struct Node";
    }
}
