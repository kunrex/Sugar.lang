using System;

using Sugar.Language.Exceptions.Analysis.Processing;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;

using Sugar.Language.Analysis.ProjectStructure.CreationNodes.Variables;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Nodes;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Variables;

namespace Sugar.Language.Analysis.ProjectStructure.LocalNodes.Variables
{
    internal class LocalVariableNode : VariableCreationNode, ILocalNode, IParentableNode<ILocalVariableParent>
    {
        public override MemberTypeEnum MemberType { get => MemberTypeEnum.Local; }
        public LocalMemberEnum LocalMember { get => LocalMemberEnum.LocalVariable; }

        protected override DescriberEnum BaseDescribers { get => DescriberEnum.Const; }

        private ILocalVariableParent parent;
        public ILocalVariableParent Parent { get => parent; }

        public LocalVariableNode(string _name, Describer _describer, DataType _type) : base(_name, _describer, _type)
        {
        }

        public void SetParent(ILocalVariableParent variableParent)
        {
            if (parent != null)
                throw new DoubleParentAssignementException();

            parent = variableParent;
        }
    }
}
