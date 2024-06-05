using System;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;

namespace Sugar.Language.Analysis.ProjectStructure.Interfaces.CreationNodes.Functions
{
    internal interface IExtension
    {
        public DataType ExtensionParent { get; }
    }
}
