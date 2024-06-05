using System;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;

namespace Sugar.Language.Analysis.ProjectStructure.Interfaces.CreationNodes
{
    internal interface IReturnable
    {
        public DataType CreationType { get; }
    }
}
