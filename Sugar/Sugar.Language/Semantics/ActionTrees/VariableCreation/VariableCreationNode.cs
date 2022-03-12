using System;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;

namespace Sugar.Language.Semantics.ActionTrees.VariableCreation
{
    internal abstract class VariableCreationNode : ParentableActionTreeNode<IVariableContainer>
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
