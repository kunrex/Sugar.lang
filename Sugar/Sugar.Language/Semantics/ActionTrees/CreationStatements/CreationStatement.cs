using System;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.Interfaces;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements
{
    internal abstract class CreationStatement<Parent> : ActionTreeNode<Parent>, IParentableCreationStatement<Parent> where Parent : IActionTreeNode 
    {
        protected readonly Describer describer;
        public Describer Describer { get => Describer; }

        protected readonly DescriberEnum allowed;
        public DescriberEnum Allowed { get => allowed; }

        protected readonly string creationName;
        public string Name { get => creationName; }

        public CreationStatement(string _creationName, Describer _describer, DescriberEnum _allowed)
        {
            creationName = _creationName;

            allowed = _allowed;
            describer = _describer;
        }

        public virtual bool ValidateDescriber() => describer.ValidateDescriber(Allowed);

        public bool CheckDescription(DescriberEnum description) => describer.CheckDescription(description);
    }
}
