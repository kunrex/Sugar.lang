using System;

using Sugar.Language.Parsing.Nodes;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;

using Sugar.Language.Analysis.ProjectStructure.CreationNodes.Functions;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Functions;

namespace Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Functions.Casting
{
    internal abstract class BaseCastNode : MethodCreationNode<ICastParent>
    {
        public override MemberTypeEnum MemberType { get => MemberTypeEnum.Global; }

        protected override DescriberEnum BaseDescribers { get => DescriberEnum.CastBaseDescriber; }

        public BaseCastNode(DataType _returnType, Describer _describer, ParseNodeCollection _body) : base(_returnType.Name, _describer, _body, _returnType)
        {
            
        }
    }
}
