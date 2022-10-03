using System;

namespace Sugar.Language.Semantics.ActionTrees.Enums
{
    internal enum GlobalMemberEnum : ushort
    {
        Variable = 0,
        Property = 1,

        Void = 2,
        Function = 4,
        ExtensionFunction = 8,

        Indexer = 16,
        Constructor = 32,

        ImplicitCast = 64,
        Explicitcast = 128,
        OperaterOverload = 256,
    }
}
