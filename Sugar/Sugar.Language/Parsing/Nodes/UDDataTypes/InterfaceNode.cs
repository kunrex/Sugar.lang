using System;

using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.UDDataTypes.Enums;

namespace Sugar.Language.Parsing.Nodes.UDDataTypes
{
    internal sealed class InterfaceNode : UDDataTypeNode
    {
        public override NodeType NodeType => NodeType.Interface;
        public override UDDataType DataType => UDDataType.Interface;

        public InterfaceNode(Node _describer, Node _name, Node _body) : base(_describer, _name, _body)
        {

        }

        public override string ToString() => $"Interface Node";
    }
}
