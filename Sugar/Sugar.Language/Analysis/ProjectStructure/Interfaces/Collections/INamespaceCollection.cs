using System;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Referencing;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.Namespaces;

namespace Sugar.Language.Analysis.ProjectStructure.Interfaces.Collections
{
    internal interface INamespaceCollection : INodeCollection<CreatedNamespaceNode, INamespaceCollection>, IReferencable
    {
        public int NameSpaceCount { get; }

        public CreatedNamespaceNode TryFindNameSpace(string value);
    }
}
