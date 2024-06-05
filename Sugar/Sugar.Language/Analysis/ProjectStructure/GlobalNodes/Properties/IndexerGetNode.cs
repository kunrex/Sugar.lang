using System;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;

using Sugar.Language.Analysis.ProjectStructure.CreationNodes.Properties;
using Sugar.Language.Analysis.ProjectStructure.CreationNodes.Properties.Structure;

namespace Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Properties
{
    internal sealed class IndexerGetNode : BaseIndexerNode
    {
        public override GlobalMemberEnum PropertyType { get => GlobalMemberEnum.PropertyGet; }

        private readonly GetNode get;
        public GetNode Get { get => get; }

        public IndexerGetNode(string _name, Describer _describer, DataType _type, GetNode _get) : base(_name, _describer, _type)
        {
            get = _get;
        }
    }
}
