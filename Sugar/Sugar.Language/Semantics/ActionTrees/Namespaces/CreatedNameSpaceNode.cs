using System;

using System.Collections.Generic;
using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.Interfaces.Collections;

namespace Sugar.Language.Semantics.ActionTrees.Namespaces
{
    internal sealed class CreatedNameSpaceNode : BaseNameSpaceNode, INameSpaceCollection
    {
        private readonly IdentifierNode name;
        public string Name { get => name.Value; }

        private readonly List<CreatedNameSpaceNode> subNamespaces;

        public int NameSpaceCount { get => subNamespaces.Count; }

        public CreatedNameSpaceNode(IdentifierNode _name) : base()
        {
            name = _name;
            subNamespaces = new List<CreatedNameSpaceNode>();
        }

        public CreatedNameSpaceNode TryFindNameSpace(IdentifierNode identifier)
        {
            foreach (var nameSpace in subNamespaces)
                if (nameSpace.Name == identifier.Value)
                    return nameSpace;

            return null;
        }

        public INameSpaceCollection AddEntity(CreatedNameSpaceNode namespaceToAdd)
        {
            subNamespaces.Add(namespaceToAdd);
            namespaceToAdd.SetParent(this);

            return this;
        }

        public CreatedNameSpaceNode GetSubNameSpace(int index) => subNamespaces[index];

        protected override void PrintChildren(string indent)
        {
            for (int i = 0; i < subNamespaces.Count; i++)
                subNamespaces[i].Print(indent, i == subNamespaces.Count - 1);

            for (int i = 0; i < dataTypes.Count; i++)
                dataTypes[i].Print(indent, i == dataTypes.Count - 1);
        }

        public override string ToString() => $"Created Name Space [{name.Value}]";
    }
}
