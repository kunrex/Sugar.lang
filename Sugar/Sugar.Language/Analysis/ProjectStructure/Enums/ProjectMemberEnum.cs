using System;

namespace Sugar.Language.Analysis.ProjectStructure.Enums
{
    [Flags]
    public enum ProjectMemberEnum : byte
    {
        Package = 1,

        DefaultNameSpace = 2,
        CreatedNameSpace = 4,
        ProjectNamespace = 8,

        Enum = 16,
        Class = 32,
        Struct = 64,
        Interface = 128,

        DataTypes = Enum | Class | Struct | Interface,
        Namespaces = DefaultNameSpace | CreatedNameSpace | ProjectNamespace,
    }
}
