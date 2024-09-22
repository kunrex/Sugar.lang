using System;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;

using Sugar.Language.Analysis.ProjectStructure.CreationNodes.Functions;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Nodes;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Functions;
using Sugar.Language.Parsing.Nodes.NodeGroups;

namespace Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Functions
{
    internal sealed class ConstructorNode : MethodCreationNode<IConstructorParent>, IGlobalNode
    {
        private readonly ExpressionListNode parentOverrideNode;
        public ExpressionListNode ParentOverrideNode { get => parentOverrideNode; }
        
        public override MemberTypeEnum MemberType { get => MemberTypeEnum.Global; }
        public GlobalMemberEnum GlobalMember { get => GlobalMemberEnum.Constructor; }

        protected override DescriberEnum BaseDescribers { get => DescriberEnum.ConstructorBaseDescriber; }

        public ConstructorNode(DataType _creationType, Describer _describer, ParseNode _body, FunctionParamatersNode _arguments) : base(_creationType.Name, _describer, _body, _creationType, _arguments)
        {

        }
        
        public ConstructorNode(DataType _creationType, Describer _describer, ParseNode _body, FunctionParamatersNode _arguments, ExpressionListNode _override) : base(_creationType.Name, _describer, _body, _creationType, _arguments)
        {
            parentOverrideNode = _override;
        }
    }
}
