using System;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;

using Sugar.Language.Exceptions.Analysis.Processing;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.CreationNodes.Functions;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Nodes;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Functions;

namespace Sugar.Language.Analysis.ProjectStructure.LocalNodes.Functions
{
    internal class LocalMethodNode : MethodCreationNode<ILocalFunctionParent>, ILocalNode, IFunctionParentable<ILocalFunctionParent, LocalVoidNode, LocalMethodNode>
    {
        public override MemberTypeEnum MemberType { get => MemberTypeEnum.Local; }
        public LocalMemberEnum LocalMember { get => LocalMemberEnum.LocalMethod; }

        protected override DescriberEnum BaseDescribers { get => 0; }

        private ILocalFunctionParent parent;
        public ILocalFunctionParent Parent { get => parent; }

        public LocalMethodNode(string _name, Describer _describer, DataType _type, ParseNodeCollection _body, FunctionParamatersNode _arguments) : base(_name, _describer, _body, _type, _arguments)
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
