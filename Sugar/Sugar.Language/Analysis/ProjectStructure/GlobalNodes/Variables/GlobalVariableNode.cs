using System;

using Sugar.Language.Exceptions.Analysis.Processing;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;

using Sugar.Language.Analysis.ProjectStructure.CreationNodes.Variables;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Nodes;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Variables;

namespace Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Variables
{
    internal sealed class GlobalVariableNode : VariableCreationNode, IGlobalNode, IParentableNode<IGlobalVariableParent>
    {
        public override MemberTypeEnum MemberType { get => MemberTypeEnum.Global; }
        public GlobalMemberEnum GlobalMember { get => GlobalMemberEnum.Variable; }

        protected override DescriberEnum BaseDescribers { get => DescriberEnum.VariableBaseDescribers; }

        private IGlobalVariableParent parent;
        public IGlobalVariableParent Parent { get => parent; }

        public GlobalVariableNode(string _name, Describer _describer, DataType _type) : base(_name, _describer, _type)
        {

        }

        public void SetParent(IGlobalVariableParent variableParent)
        {
            if (parent != null)
                throw new DoubleParentAssignementException();

            parent = variableParent;
        }
    }
}
