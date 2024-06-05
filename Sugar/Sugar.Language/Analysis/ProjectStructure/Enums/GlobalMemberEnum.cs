using System;

namespace Sugar.Language.Analysis.ProjectStructure.Enums
{
    [Flags]
    public enum GlobalMemberEnum : ushort
    {
        Variable = 1,

        Void = 2,
        Method = 4,
        Extension = 8,

        PropertyGet = 16,
        PropertySet = 32,
        PropertyGetSet = 64,

        Indexer = 128,
        Constructor = 256,

        ImplicitCast = 512,
        ExplicitCast = 1024,
        OperaterOverload = 2045,

        PropertiesExtended = Indexer | Properties,
        Properties = PropertyGet | PropertySet | PropertyGetSet,

        DataTypeMembers = Indexer | ImplicitCast | ExplicitCast,
        Functions = Void | Method | Extension | Constructor | ImplicitCast | ExplicitCast
    }
}
