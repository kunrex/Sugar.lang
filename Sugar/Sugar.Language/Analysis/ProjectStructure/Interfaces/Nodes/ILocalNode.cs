using System;

using Sugar.Language.Analysis.ProjectStructure.Enums;

namespace Sugar.Language.Analysis.ProjectStructure.Interfaces.Nodes
{
    internal interface ILocalNode : INode
    {
        public LocalMemberEnum LocalMember { get; }
    }
}
