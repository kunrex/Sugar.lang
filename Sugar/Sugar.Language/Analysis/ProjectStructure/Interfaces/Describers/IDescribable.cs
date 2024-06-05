using System;

namespace Sugar.Language.Analysis.ProjectStructure.Interfaces.Describers
{
    internal interface IDescribable
    {
        public Describer Describers { get; }

        public bool ValidateDescribers();
        public bool MatchDescriber(Describer describer);
    }
}
