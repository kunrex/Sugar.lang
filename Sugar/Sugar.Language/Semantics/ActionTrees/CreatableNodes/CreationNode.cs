﻿using System;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.Interfaces;
using Sugar.Language.Semantics.ActionTrees.Interfaces.Describers;

namespace Sugar.Language.Semantics.ActionTrees.CreatableNodes
{
    internal abstract class CreationNode<Parent> : ParentableActionTreeNode<Parent>, IDescribable where Parent : IActionTreeNode
    {
        protected readonly Describer describer;
        public Describer Describer { get => Describer; }

        protected readonly DescriberEnum allowed;
        public DescriberEnum Allowed { get => allowed; }

        public DataType CreationType { get; protected set; }

        protected readonly IdentifierNode creationName;
        public string Name { get => creationName.Value; }

        public CreationNode(DataType _creationType, IdentifierNode _creationName, Describer _describer, DescriberEnum _allowed)
        {
            CreationType = _creationType;
            creationName = _creationName;

            allowed = _allowed;
            describer = _describer;
        }

        public virtual bool ValidateDescriber() => describer.ValidateAccessor(Allowed);
    }
}
