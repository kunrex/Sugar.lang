using System;
using Sugar.Language.Parsing.Nodes.UDDataTypes.Enums;

using Sugar.Language.Parsing.Nodes.Enums;

namespace Sugar.Language.Parsing.Nodes.UDDataTypes
{
    internal sealed class ClassNode : UDDataTypeNode
    {
        public override NodeType NodeType => NodeType.Class;
        public override UDDataType DataType => UDDataType.Class;

        public ClassNode(Node _describer, Node _name, Node _body) : base(_describer, _name, _body)
        {

        }

        public override string ToString() => $"Class Node";
    }
}
