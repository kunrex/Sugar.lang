using System;

using Sugar.Language.Analysis.ProjectStructure.Enums;

namespace Sugar.Language.Analysis.ProjectStructure.Interfaces
{
    internal interface INode
    {
        public MemberTypeEnum MemberType { get; }
    }
}
