using System;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.CreationNodes;

namespace Sugar.Language.Analysis.ProjectStructure.CreationNodes.Variables
{
    internal abstract class VariableCreationNode : CreationNode, IReturnable
    {
        protected readonly DataType creationType;
        public DataType CreationType { get => creationType; }

        public VariableCreationNode(string _name, Describer _describer, DataType _type) : base(_name, _describer)
        {
            creationType = _type;
        }
    }
}
