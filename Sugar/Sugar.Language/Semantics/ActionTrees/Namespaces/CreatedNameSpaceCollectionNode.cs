﻿using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.Interfaces.Namespaces;

namespace Sugar.Language.Semantics.ActionTrees.Namespaces
{
    internal sealed class CreatedNameSpaceCollectionNode : INameSpaceCollection
    {
        private readonly List<CreatedNameSpaceNode> namespaces;

        public CreatedNameSpaceCollectionNode()
        {
            namespaces = new List<CreatedNameSpaceNode>();
        }

        public CreatedNameSpaceNode TryFindNameSpace(IdentifierNode identifier)
        {
            foreach (var nameSpace in namespaces)
                if (nameSpace.Name == identifier.Value)
                    return nameSpace;

            return null;
        }

        public INameSpaceCollection AddNameSpace(CreatedNameSpaceNode namespaceToAdd)
        {
            namespaces.Add(namespaceToAdd);

            return this;
        }

        public void Print(string indent)
        {
            for (int i = 0; i < namespaces.Count; i++)
                namespaces[i].Print(indent, i == namespaces.Count - 1);
        }
    }
}