using System;

using Sugar.Language.Exceptions.Analysis.Processing;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.CreationNodes.Functions;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Nodes;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Functions;

namespace Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Functions
{
    internal class GlobalVoidNode : VoidCreationNode<IGlobalFunctionParent>, IGlobalNode, IFunctionParentable<IGlobalFunctionParent, GlobalVoidNode, GlobalMethodNode>
    {
        public override MemberTypeEnum MemberType { get => MemberTypeEnum.Global; }
        public GlobalMemberEnum GlobalMember { get => GlobalMemberEnum.Void; }

        protected override DescriberEnum BaseDescribers { get => DescriberEnum.GlobalFunctionBaseDescriber; }

        private IGlobalFunctionParent parent;
        public IGlobalFunctionParent Parent { get => parent; }

        public GlobalVoidNode(string _name, Describer _describer, ParseNode _body, FunctionParamatersNode _arguments) : base(_name, _describer, _body, _arguments)
        {

        }

        public void SetParent(IGlobalFunctionParent functionParent)
        {
            if(parent != null)
                throw new DoubleParentAssignementException();

            parent = functionParent;
        }
    }
}
