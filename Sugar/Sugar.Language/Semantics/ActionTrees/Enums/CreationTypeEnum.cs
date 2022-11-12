using System;

namespace Sugar.Language.Semantics.ActionTrees.Enums
{
    internal enum CreationTypeEnum : byte
    {
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
