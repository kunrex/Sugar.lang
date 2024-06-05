using System;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Referencing;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;

namespace Sugar.Language.Analysis.ProjectStructure.Interfaces.Collections
{
    internal interface IDataTypeCollection : INodeCollection<DataType, IDataTypeCollection>, IReferencable
    {
        public int DataTypeCount { get; }

        public DataType TryFindDataType(IdentifierNode identifier);
    }
}
