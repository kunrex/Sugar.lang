using System;

using Sugar.Language.Parsing.Nodes;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.CreationNodes;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;

namespace Sugar.Language.Analysis.ProjectStructure.CreationNodes.Functions.Structure
{
    internal sealed class FunctionParameter : CreationNode, IReturnable
    {
        public override MemberTypeEnum MemberType { get => MemberTypeEnum.Local; }

        private readonly DataType creationType;
        public DataType CreationType { get => creationType; }

        protected override DescriberEnum BaseDescribers { get => DescriberEnum.ReferenceModifiers; }

        private readonly ParseNode defaultValue;
        public ParseNode DefaultValue { get => defaultValue; }
        
        public FunctionParameter(string _name, DataType _type, Describer _describer) : base(_name, _describer)
        {
            creationType = _type;
            defaultValue = null;
        }
        
        public FunctionParameter(string _name, DataType _type, Describer _describer, ParseNode _default) : base(_name, _describer)
        {
            creationType = _type;
            defaultValue = _default;
        }
    }
}
