using System;
using System.Linq;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Collections;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Referencing;

namespace Sugar.Language.Analysis.ProjectStructure.ProjectNodes.Namespaces
{
    internal sealed class ProjectNamespaceNode : CollectionProjectNode<CreatedNamespaceNode>, INamespaceCollection, IReferencable
    {
        public override ProjectMemberEnum ProjectMemberType { get => ProjectMemberEnum.ProjectNamespace; }

        public int NameSpaceCount { get => Length; }

        public override CreatedNamespaceNode this[int index] { get => children.ElementAt(index).Value; }

        public ProjectNamespaceNode() : base("project")
        {
            
        }

        public CreatedNamespaceNode TryFindNameSpace(string value)
        {
            children.TryGetValue(value, out var val);
            return val;
        }

        public INamespaceCollection AddEntity(CreatedNamespaceNode nameSpace)
        {
            children.Add(nameSpace.Name, nameSpace);

            return this;
        }

        public IReferencable GetParent() { throw new NotImplementedException(); }

        public IReferencable[] GetChildReference(string value)
        {
            foreach (var child in children)
                if (child.Key == value)
                    return new IReferencable[] { child.Value };

            return null;
        }

        public override string ToString() => $"Project Namespace";

        protected override void PrintChildren(string indent)
        {
            for (int i = 0; i < children.Count; i++)
                children.ElementAt(i).Value.Print(indent, i == children.Count - 1);
        }
    }
}
