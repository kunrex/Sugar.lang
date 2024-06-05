using System;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.CreationNodes;

namespace Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting.Properties
{
    internal interface IIndexerParent : IPropertyParent
    {
        public IPropertyParent AddIndexer(IIndexer indexer);

        public IIndexer TryFindSetIndexer(IdentifierNode identifier);
        public IIndexer TryFindGetIndexer(IdentifierNode identifier);
    }
}
