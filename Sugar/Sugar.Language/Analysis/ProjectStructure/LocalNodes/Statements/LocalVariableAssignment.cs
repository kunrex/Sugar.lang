using System;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Nodes;

namespace Sugar.Language.Analysis.ProjectStructure.LocalNodes.Statements
{
    internal sealed class LocalVariableAssignment : ILocalNode
    {
        public MemberTypeEnum MemberType { get => MemberTypeEnum.Local; }
        public LocalMemberEnum LocalMember { get => LocalMemberEnum.LocalVariableAssignment; }
       

        public LocalVariableAssignment()
        {

        }

       
    }
}
