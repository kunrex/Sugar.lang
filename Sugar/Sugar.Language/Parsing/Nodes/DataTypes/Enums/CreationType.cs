using System;

using Sugar.Language.Analysis.ProjectStructure.Enums;

namespace Sugar.Language.Parsing.Nodes.DataTypes.Enums
{
    internal enum CreationType : ushort
    {
        Enum = ProjectMemberEnum.Enum,
        Class = ProjectMemberEnum.Class,
        Struct = ProjectMemberEnum.Struct,
        Interface = ProjectMemberEnum.Interface,
        Namespace = ProjectMemberEnum.CreatedNameSpace
    }
}
