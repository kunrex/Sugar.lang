using System;

using Sugar.Language.Parsing.Nodes;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.LocalNodes.Variables;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;

namespace Sugar.Language.Analysis.ProjectStructure.CreationNodes.Properties.Structure
{
    internal sealed class Set : AccessoryNode
    {
        public Set(Describer _describer, ParseNode _body, DataType _type) : base(_describer, _body)
        {
            AddVariable(new LocalVariableNode("value", new Describer(DescriberEnum.Const), _type));
        }
    }
}
