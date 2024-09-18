using System;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Referencing;

namespace Sugar.Language.Analysis.ProjectCreation.Utilities
{
    internal struct FindResult
    {
        private readonly int index;
        public int Index { get => index; }

        private readonly IReferencable referencable;
        public IReferencable Referencable { get => referencable; }

        public FindResult(int _index, IReferencable _referencable)
        {
            index = _index;
            referencable = _referencable;
        }
    }
}
