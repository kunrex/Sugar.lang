using System;

using Sugar.Language.Analysis.ProjectStructure.Enums;

namespace Sugar.Language.Analysis.ProjectStructure.Interfaces.Nodes
{
    internal interface IProjectNode : INode
    {
        public ProjectMemberEnum ProjectMemberType { get; }
    }
}
