using System;

using Sugar.Language.Semantics.ActionTrees.Namespaces;

namespace Sugar.Language.Semantics.Analysis.Structure
{
    internal struct FindNamespaceResult
    {
        private bool completeValidation;
        public bool CompleteValidation { get => completeValidation; }

        private CreatedNameSpaceNode createdNameSpace;
        public CreatedNameSpaceNode CreatedNameSpace { get => createdNameSpace; }

        public FindNamespaceResult(bool _complete, CreatedNameSpaceNode _found)
        {
            completeValidation = _complete;
            createdNameSpace = _found;
        }
    }
}
