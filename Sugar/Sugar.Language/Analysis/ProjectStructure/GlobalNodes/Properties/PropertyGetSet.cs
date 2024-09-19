using System;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;

using Sugar.Language.Analysis.ProjectStructure.CreationNodes.Properties;
using Sugar.Language.Analysis.ProjectStructure.CreationNodes.Properties.Structure;

namespace Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Properties
{
    internal class PropertyGetSet : BasePropertyNode
    {
        public override GlobalMemberEnum GlobalMember { get => GlobalMemberEnum.PropertyGetSet; }

        private readonly Get get;
        public Get Get { get => get; }

        private readonly Set set;
        public Set Set { get => set; }

        public PropertyGetSet(string _name, Describer _describer, DataType _type, Get _get, Set _set) : base(_name, _describer, _type)
        {
            get = _get;
            set = _set;
        }
    }
}
