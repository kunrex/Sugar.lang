using System;

using Sugar.Language.Analysis.ProjectStructure.Enums;

namespace Sugar.Language.Analysis.ProjectStructure.Interfaces.Nodes
{
    internal interface IGlobalNode : INode
    {
        public GlobalMemberEnum GlobalMember { get; }
    }
}
