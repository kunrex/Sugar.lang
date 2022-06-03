using System;
using System.Collections.Generic;

using Sugar.Language.Semantics.ActionTrees.Namespaces;
using Sugar.Language.Semantics.ActionTrees.Interfaces.Collections;
using Sugar.Language.Parsing.Nodes.Values;

namespace Sugar.Language.Semantics.ActionTrees
{
    internal sealed class SugarPackage : INameSpaceCollection
    {
        private readonly DefaultNameSpaceNode defaultNameSpaceNode;
        public DefaultNameSpaceNode DefaultNameSpace { get => defaultNameSpaceNode; }

        private readonly CreatedNameSpaceCollectionNode createdNameSpaces;
        public CreatedNameSpaceCollectionNode CreatedNameSpaces { get => createdNameSpaces; }

        public int NameSpaceCount { get => 2; }

        public SugarPackage(DefaultNameSpaceNode _defaultNameSpaceNode, CreatedNameSpaceCollectionNode _createdNameSpaces)
        {
            createdNameSpaces = _createdNameSpaces;
            defaultNameSpaceNode = _defaultNameSpaceNode;
        }

        public CreatedNameSpaceNode GetSubNameSpace(int index) => throw new NotImplementedException();

        public INameSpaceCollection AddEntity(CreatedNameSpaceNode dataType) => throw new NotImplementedException();

        public CreatedNameSpaceNode TryFindNameSpace(IdentifierNode identifier) => throw new NotImplementedException();
    }
}
