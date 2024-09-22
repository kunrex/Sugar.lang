using System;

using Sugar.Language.Parsing.Nodes.Functions.Declarations.Structure;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;

using Sugar.Language.Analysis.ProjectStructure.CreationNodes.Properties;
using Sugar.Language.Analysis.ProjectStructure.CreationNodes.Properties.Structure;

namespace Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Properties
{
    internal sealed class IndexerGetSet : BaseIndexerNode
    {
        public override GlobalMemberEnum PropertyType { get => GlobalMemberEnum.PropertyGet; }

        private readonly Get get;
        public Get Get { get => get; }

        private readonly Set set;
        public Set Set { get => set; }

        public IndexerGetSet(Describer _describer, DataType _type, Get _get, Set _set, FunctionParamatersNode _arguments) : base(_describer, _type, _arguments)
        {
            get = _get;
            set = _set;
        }
    }
}
