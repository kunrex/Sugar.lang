using System;

namespace Sugar.Language.Analysis.ProjectStructure.Enums
{
    [Flags]
    public enum ProjectMemberEnum : ushort
    {
        Package = 1,

        DefaultNameSpace = 2,
        CreatedNameSpace = 4,
        ProjectNamespace = 8,

        Enum = 16,
        Class = 32,
        Struct = 64,
        Interface = 128,
        
        InvalidDataType = 256,

        DataTypes = Enum | Class | Struct | Interface | InvalidDataType,
        Namespaces = DefaultNameSpace | CreatedNameSpace | ProjectNamespace,
    }
}
