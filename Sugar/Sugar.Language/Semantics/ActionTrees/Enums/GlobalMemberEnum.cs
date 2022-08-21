using System;

namespace Sugar.Language.Semantics.ActionTrees.Enums
{
    internal enum GlobalMemberEnum : ushort
    {
        Variable = 0,
        Property = 1,

        Function = 2,
        ExtensionFunction = 4,

        Indexer = 8,
        Constructor = 16,

        ImplicitCast = 32,
        Explicitcast = 64,
        OperaterOverload = 128,
    }
}
