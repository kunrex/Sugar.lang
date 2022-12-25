using System;

using Sugar.Language.Parsing.Nodes.UDDataTypes.Enums;

namespace Sugar.Language.Semantics.ActionTrees.Enums
{
    internal enum ActionNodeEnum : byte
    {
        Scope,
        Package,

        Enum = UDDataType.Enum,
        Class = UDDataType.Class,
        Struct = UDDataType.Struct,
        Interface = UDDataType.Interface,
        Namespace = UDDataType.Namespace,
        DefaultNameSpace,
        NameSpaceCollection,

        LocalVariable,
        GlobalVariable,
        FunctionArgument,

        PropertyGet = PropertyTypeEnum.Get,
        PropertySet = PropertyTypeEnum.Set,
        PropertyGetSet = PropertyTypeEnum.GetSet,

        LocalVoid,
        LocalFunction,

        GlobalVoid,
        GlobalFunction,

        ExtensionVoid,
        ExtensionFunction,

        Indexer,
        Constructor,
        OperatorOverload,

        ExplicitCast,
        ImplicitCast
    }
}
