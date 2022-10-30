using System;

namespace Sugar.Language.Semantics.ActionTrees.Enums
{
    internal enum MemberEnum : ushort
    {
        Variable = 1,
        Property = 2,

        Void = 4,
        Function = 8,
        ExtensionFunction = 16,

        Indexer = 32,
        Constructor = 64,

        ImplicitCast = 128,
        ExplicitCast = 256,
        OperaterOverload = 512,

        Properties = Indexer | Property,
        DataTypeMembers = Indexer | ImplicitCast | ExplicitCast,
        Functions = Void | Function | ExtensionFunction | Constructor | ImplicitCast | ExplicitCast
    }
}
