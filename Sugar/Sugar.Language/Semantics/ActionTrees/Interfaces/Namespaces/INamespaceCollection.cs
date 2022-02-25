using System;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.Namespaces;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces.Namespaces
{
    internal interface INameSpaceCollection 
    {
        public CreatedNameSpaceNode TryFindNameSpace(IdentifierNode identifier);
        public INameSpaceCollection AddNameSpace(CreatedNameSpaceNode namespaceToAdd);
    }
}
