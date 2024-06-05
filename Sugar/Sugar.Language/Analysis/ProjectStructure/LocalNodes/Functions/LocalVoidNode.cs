using System;

using Sugar.Language.Parsing.Nodes;

using Sugar.Language.Exceptions.Analysis.Processing;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.CreationNodes.Functions;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Nodes;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Functions;

namespace Sugar.Language.Analysis.ProjectStructure.LocalNodes.Functions
{
    internal class LocalVoidNode : VoidCreationNode<ILocalFunctionParent>, ILocalNode, IFunctionParentable<ILocalFunctionParent, LocalVoidNode, LocalMethodNode>
    {
        public override MemberTypeEnum MemberType { get => MemberTypeEnum.Local; }
        public LocalMemberEnum LocalMember { get => LocalMemberEnum.LocalVoid; }

        protected override DescriberEnum BaseDescribers { get => 0; }

        private ILocalFunctionParent parent;
        public ILocalFunctionParent Parent { get => parent; }

        public LocalVoidNode(string _name, Describer _describer, ParseNodeCollection _body) : base(_name, _describer, _body)
        {

        }

        public void SetParent(ILocalFunctionParent functionParent)
        {
            if (parent != null)
                throw new DoubleParentAssignementException();

            parent = functionParent;
        }
    }
}
