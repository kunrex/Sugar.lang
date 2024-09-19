using System;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;

using Sugar.Language.Analysis.ProjectStructure.CreationNodes.Properties;
using Sugar.Language.Analysis.ProjectStructure.CreationNodes.Properties.Structure;

namespace Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Properties
{
    internal sealed class IndexerGet : BaseIndexerNode
    {
        public override GlobalMemberEnum PropertyType { get => GlobalMemberEnum.PropertyGet; }

        private readonly Get get;
        public Get Get { get => get; }

        public IndexerGet(Describer _describer, DataType _type, Get _get) : base(_describer, _type)
        {
            get = _get;
        }
    }
}
