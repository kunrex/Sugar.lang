using System;
using System.Collections;
using System.Collections.Generic;

using Sugar.Language.Services;
using Sugar.Language.Services.Interfaces;

using Sugar.Language.Exceptions;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Nodes;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.Namespaces;

namespace Sugar.Language.Analysis.ProjectStructure.ProjectNodes
{
    internal sealed class ProjectTree : ProjectNode, ICustomCollection<CompileException>, IProjectNode
    {
        public override ProjectMemberEnum ProjectMemberType { get => ProjectMemberEnum.Package; }

        private DefaultNamespaceNode defaultNamespace;
        public DefaultNamespaceNode DefaultNamespace { get => defaultNamespace; }

        private ProjectNamespaceNode projectNamespace;
        public ProjectNamespaceNode ProjectNamespace { get => projectNamespace; }

        private readonly List<CompileException> exceptions;
        public int Length { get => exceptions.Count; }

        public CompileException this[int index] { get => throw new NotImplementedException(); }

        public ProjectTree(string _name) : base(_name)
        {
            defaultNamespace = new DefaultNamespaceNode();
            projectNamespace = new ProjectNamespaceNode();

            exceptions = new List<CompileException>();
        }

        public ProjectTree WithException(CompileException exception)
        {
            exceptions.Add(exception);

            return this;
        }

        public IEnumerator<CompileException> GetEnumerator()
        {
            return new GenericEnumeratorService<ProjectTree, CompileException>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString() => $"Project Tree [Name: {name}]";

        protected override void PrintChildren(string indent)
        {
            defaultNamespace.Print(indent, false);
            projectNamespace.Print(indent, true);
        }
    }
}
