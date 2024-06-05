using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Collections;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Referencing;

namespace Sugar.Language.Analysis.ProjectStructure.ProjectNodes.Namespaces
{
    internal class CreatedNamespaceNode : BaseNamespaceNode, INamespaceCollection, IReferencable
    {
        private readonly List<CreatedNamespaceNode> namespaces;
        public IReadOnlyCollection<CreatedNamespaceNode> Namespaces { get => namespaces; }

        public int NameSpaceCount { get => namespaces.Count; }

        public override ProjectMemberEnum ProjectMemberType { get => ProjectMemberEnum.CreatedNameSpace; }

        public CreatedNamespaceNode(string _name) : base(_name)
        {
            namespaces = new List<CreatedNamespaceNode>();
        }

        public override void SetParent(INamespaceCollection _parent)
        {
            base.SetParent(_parent);

            foreach (var child in namespaces)
                child.SetParent(this);
        }

        public INamespaceCollection AddEntity(CreatedNamespaceNode nameSpace)
        {
            namespaces.Add(nameSpace);

            return this;
        }

        public CreatedNamespaceNode TryFindNameSpace(IdentifierNode identifier)
        {
            var value = identifier.Value;

            foreach (var name in namespaces)
                if (name.Name == value)
                    return name;

            return null;
        }

        public override IReferencable GetParent() { return parent; }

        public override IReferencable[] GetChildReference(string value)
        {
            var set = new List<IReferencable>();

            foreach (var child in namespaces)
                if (child.Name == value)
                {
                    set.Add(child);
                    break;
                }

            foreach (var child in children)
                if (child.Name == value)
                {
                    set.Add(child);
                    break;
                }

            return set.ToArray();
        }

        public override string ToString() => $"Created Namespace [Name: {name}]";

        protected override void PrintChildren(string indent)
        {
            for (int i = 0; i < children.Count; i++)
                children[i].Print(indent, i == children.Count - 1);

            for (int i = 0; i < namespaces.Count; i++)
                namespaces[i].Print(indent, i == namespaces.Count - 1);
        }
    }
}
