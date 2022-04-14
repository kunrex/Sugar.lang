using System;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.Namespaces;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces.Collections
{
    internal interface INameSpaceCollection : IEntityCollection<CreatedNameSpaceNode, INameSpaceCollection>
    {
        public int NameSpaceCount { get; }

        public CreatedNameSpaceNode GetSubNameSpace(int index);
        public CreatedNameSpaceNode TryFindNameSpace(IdentifierNode identifier);
    }
}
