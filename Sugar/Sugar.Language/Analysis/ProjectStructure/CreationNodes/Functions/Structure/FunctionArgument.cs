using System;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.CreationNodes;

using Sugar.Language.Analysis.ProjectStructure.Interfaces;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Describers;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.CreationNodes;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;

namespace Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Functions.Structure
{
    internal sealed class FunctionArgument : CreationNode, INameable, IDescribable, IReturnable
    {
        public override MemberTypeEnum MemberType { get => MemberTypeEnum.Local; }

        private readonly DataType creationType;
        public DataType CreationType { get => creationType; }

        protected override DescriberEnum BaseDescribers { get => DescriberEnum.ReferenceModifiers; }

        public FunctionArgument(string _name, DataType _type, Describer _describer) : base(_name, _describer)
        {
            creationType = _type;
        }
    }
}
