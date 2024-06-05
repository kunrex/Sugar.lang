using System;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;


using Sugar.Language.Analysis.ProjectStructure.CreationNodes.Properties;
using Sugar.Language.Analysis.ProjectStructure.CreationNodes.Properties.Structure;

namespace Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Properties
{
    internal sealed class PropertySetNode : BasePropertyNode
    {
        public override GlobalMemberEnum GlobalMember { get => GlobalMemberEnum.PropertySet; }

        private readonly SetNode set;
        public SetNode Set { get => set; }

        public PropertySetNode(string _name, Describer _describer, DataType _type, SetNode _set) : base(_name, _describer, _type)
        {
            set = _set;
        }
    }
}
