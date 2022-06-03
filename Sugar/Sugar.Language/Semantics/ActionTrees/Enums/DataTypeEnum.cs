using System;

using Sugar.Language.Parsing.Nodes.UDDataTypes.Enums;

namespace Sugar.Language.Semantics.ActionTrees.Enums
{
    public enum DataTypeEnum : byte
    {
        Enum = UDDataType.Enum,
        Class = UDDataType.Class,
        Struct = UDDataType.Struct,
        Interface = UDDataType.Interface,
        Namespace = UDDataType.Namespace
    }
}
