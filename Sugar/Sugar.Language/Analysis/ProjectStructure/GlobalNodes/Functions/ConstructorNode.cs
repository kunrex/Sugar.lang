using System;

using Sugar.Language.Parsing.Nodes;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;

using Sugar.Language.Analysis.ProjectStructure.CreationNodes.Functions;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Nodes;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Functions;

namespace Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Functions
{
    internal sealed class ConstructorNode : MethodCreationNode<IConstructorParent>, IGlobalNode
    {
        public override MemberTypeEnum MemberType { get => MemberTypeEnum.Global; }
        public GlobalMemberEnum GlobalMember { get => GlobalMemberEnum.Constructor; }

        protected override DescriberEnum BaseDescribers { get => DescriberEnum.ConstructorBaseDescriber; }

        public ConstructorNode(DataType _creationType, Describer _describer, ParseNode _body) : base(_creationType.Name, _describer, _body, _creationType)
        {

        }
    }
}
