using System;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.Namespaces;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces.Namespaces
{
    internal interface INameSpaceCollection : IActionTreeNode
    {
        public int NameSpaceCount { get; }

        public CreatedNameSpaceNode this[int index] { get; }

        public CreatedNameSpaceNode TryFindNameSpace(IdentifierNode identifier);
        public INameSpaceCollection AddNameSpace(CreatedNameSpaceNode namespaceToAdd);
    }
}
