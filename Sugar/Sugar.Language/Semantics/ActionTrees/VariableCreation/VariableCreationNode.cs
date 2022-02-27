using System;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.DataTypes;

namespace Sugar.Language.Semantics.ActionTrees.VariableCreation
{
    internal abstract class VariableCreationNode : ActionTreeNode
    {
        public DataType CreationType { get; protected set; }
        public IdentifierNode CreationName { get; protected set; }

        public VariableCreationNode(DataType _creationType, IdentifierNode _creationName)
        {
            CreationType = _creationType;
            CreationName = _creationName;
        }
    }
}
