using System;
using System.Collections.Generic;

using Sugar.Language.Semantics.ActionTrees.Namespaces;

namespace Sugar.Language.Semantics.ActionTrees
{
    internal sealed class SugarPackage
    {
        private readonly DefaultNameSpaceNode defaultNameSpaceNode;
        public DefaultNameSpaceNode DefaultNameSpace { get => defaultNameSpaceNode; }

        private readonly CreatedNameSpaceCollectionNode createdNameSpaces;
        public CreatedNameSpaceCollectionNode CreatedNameSpaces { get => createdNameSpaces; }

        public SugarPackage(DefaultNameSpaceNode _defaultNameSpaceNode, CreatedNameSpaceCollectionNode _createdNameSpaces)
        {
            createdNameSpaces = _createdNameSpaces;
            defaultNameSpaceNode = _defaultNameSpaceNode;
        }
    }
}
