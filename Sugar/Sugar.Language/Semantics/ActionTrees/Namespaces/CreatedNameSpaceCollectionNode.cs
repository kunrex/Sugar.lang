﻿using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.Interfaces;
using Sugar.Language.Semantics.ActionTrees.Interfaces.Collections;

namespace Sugar.Language.Semantics.ActionTrees.Namespaces
{
    internal sealed class CreatedNameSpaceCollectionNode : INameSpaceCollection, IPrintable
    {
        private readonly List<CreatedNameSpaceNode> namespaces;

        public int NameSpaceCount { get => namespaces.Count; }

        public ActionNodeEnum ActionNodeType { get => ActionNodeEnum.NameSpaceCollection; }

        public CreatedNameSpaceCollectionNode()
        {
            namespaces = new List<CreatedNameSpaceNode>();
        }

        public CreatedNameSpaceNode GetSubNameSpace(int index) => namespaces[index];

        public CreatedNameSpaceNode TryFindNameSpace(IdentifierNode identifier)
        {
            foreach (var nameSpace in namespaces)
                if (nameSpace.Name == identifier.Value)
                    return nameSpace;

            return null;
        }

        public INameSpaceCollection AddEntity(CreatedNameSpaceNode namespaceToAdd)
        {
            namespaces.Add(namespaceToAdd);
            namespaceToAdd.SetParent(this);

            return this;
        }

        public void Print(string indent, bool last)
        {
            for (int i = 0; i < namespaces.Count; i++)
                namespaces[i].Print(indent, i == namespaces.Count - 1);
        }
    }
}
