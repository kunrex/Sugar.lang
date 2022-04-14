using System;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;

namespace Sugar.Language.Semantics.ActionTrees.CreatableNodes.VariableCreation
{
    internal abstract class VariableCreationNode : CreationNode<IVariableContainer>
    {
        public VariableCreationNode(DataType _creationType, IdentifierNode _creationName, Describer _describer, DescriberEnum _allowed) : base(
            _creationType,
            _creationName,
            _describer,
            _allowed)
        {

        }
    }
}
