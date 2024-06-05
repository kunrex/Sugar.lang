using System;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Describers;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.CreationNodes;

namespace Sugar.Language.Analysis.ProjectStructure.CreationNodes
{
    internal abstract class CreationNode : ICreationNode, IDescribable
    {
        protected readonly string name;
        public string Name { get => name; }

        public abstract MemberTypeEnum MemberType { get; }

        protected readonly Describer describer;
        public Describer Describers { get => describer; }

        protected abstract DescriberEnum BaseDescribers { get; }

        public CreationNode(string _name, Describer _describer)
        {
            name = _name;
            describer = _describer;
        }

        public bool ValidateDescribers() => describer.ValidateDescription(BaseDescribers);
        public bool MatchDescriber(Describer tomatch) => tomatch.ValidateDescriber(describer);
    }
}
