using System;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Interfaces.Describers;

namespace Sugar.Language.Semantics.ActionTrees.VariableCreation
{
    internal abstract class VariableCreationNode : ParentableActionTreeNode<IVariableContainer>, IDescribable
    {
        public DataType CreationType { get; protected set; }
        public IdentifierNode CreationName { get; protected set; }

        protected readonly Describer describer;
        public Describer Describer { get => Describer; }

        public abstract DescriberEnum Allowed { get; }

        public VariableCreationNode(DataType _creationType, IdentifierNode _creationName, Describer _describer)
        {
            CreationType = _creationType;
            CreationName = _creationName;

            describer = _describer;
        }

        public virtual bool ValidateDescriber() => describer.ValidateAccessor(Allowed);
    }
}
