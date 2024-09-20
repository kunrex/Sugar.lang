using System;

using Sugar.Language.Parsing.Nodes;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Value;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;

using Sugar.Language.Analysis.ProjectStructure.CreationNodes.Properties.Structure;

namespace Sugar.Language.Analysis.ProjectStructure.GlobalNodes.Properties.Initialise
{
    internal sealed class SetInitialise : PropertySet, IValueNode
    {
        private readonly ParseNode value;
        public ParseNode Value { get => value; }
    
        public SetInitialise(string _name, Describer _describer, DataType _type, Set _set, ParseNode _value) : base(_name, _describer, _type, _set)
        {
            value = _value;
        }
    }
}

