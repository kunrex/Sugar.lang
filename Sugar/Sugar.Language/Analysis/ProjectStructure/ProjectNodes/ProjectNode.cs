using System;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.Interfaces;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Nodes;

namespace Sugar.Language.Analysis.ProjectStructure.ProjectNodes
{
    internal abstract class ProjectNode : BaseNode, INameable, IProjectNode
    {
        protected readonly string name;
        public string Name { get => name; }

        public override MemberTypeEnum MemberType { get => MemberTypeEnum.Project; }

        public abstract ProjectMemberEnum ProjectMemberType { get; }

        public ProjectNode(string _name)
        {
            name = _name;
        }
    }
}
