using System;
using System.Linq;
using System.Collections.Generic;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Collections;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Referencing;

namespace Sugar.Language.Analysis.ProjectStructure.ProjectNodes.Namespaces
{
    internal class CreatedNamespaceNode : BaseNamespaceNode, INamespaceCollection, IReferencable
    {
        private readonly Dictionary<string, CreatedNamespaceNode> namespaces;
        public IReadOnlyCollection<CreatedNamespaceNode> Namespaces { get => namespaces.Values; }

        public int NameSpaceCount { get => namespaces.Count; }

        public override ProjectMemberEnum ProjectMemberType { get => ProjectMemberEnum.CreatedNameSpace; }

        public CreatedNamespaceNode(string _name) : base(_name)
        {
            namespaces = new Dictionary<string, CreatedNamespaceNode>();
        }

        public override void SetParent(INamespaceCollection _parent)
        {
            base.SetParent(_parent);

            foreach (var child in namespaces)
                child.Value.SetParent(this);
        }

        public INamespaceCollection AddEntity(CreatedNamespaceNode nameSpace)
        {
            namespaces.Add(nameSpace.Name, nameSpace);

            return this;
        }

        public CreatedNamespaceNode TryFindNameSpace(string value)
        {
            namespaces.TryGetValue(value, out var val);
            return val;
        }

        public override IReferencable GetParent() { return parent; }

        public override IReferencable[] GetChildReference(string value)
        {
            var set = new List<IReferencable>();

            foreach (var child in namespaces)
                if (child.Key == value)
                {
                    set.Add(child.Value);
                    break;
                }

            foreach (var child in children)
                if (child.Key == value)
                {
                    set.Add(child.Value);
                    break;
                }

            return set.ToArray();
        }

        public override string ToString() => $"Created Namespace [Name: {name}]";

        protected override void PrintChildren(string indent)
        {
            for (int i = 0; i < children.Count; i++)
                children.ElementAt(i).Value.Print(indent, i == children.Count - 1);

            for (int i = 0; i < namespaces.Count; i++)
                namespaces.ElementAt(i).Value.Print(indent, i == namespaces.Count - 1);
        }
    }
}
