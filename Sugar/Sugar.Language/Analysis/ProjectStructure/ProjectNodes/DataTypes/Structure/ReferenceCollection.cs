using System;
using System.Collections;
using System.Collections.Generic;

using Sugar.Language.Services;
using Sugar.Language.Services.Interfaces;

using Sugar.Language.Parsing.Nodes.Statements;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Referencing;

namespace Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes.Structure
{
    internal sealed class ReferenceCollection : ICustomCollection<IReferencable>
    {
        private readonly List<ImportNode> imports;
        public IReadOnlyCollection<ImportNode> Imports { get => imports; }

        private readonly List<IReferencable> references;

        public int Length { get => references.Count; }
        public IReferencable this[int index] { get => references[index]; }

        public ReferenceCollection()
        {
            imports = new List<ImportNode>();
            references = new List<IReferencable>();
        }

        public ReferenceCollection(ReferenceCollection _references)
        {
            imports = new List<ImportNode>(_references.imports);
            references = new List<IReferencable>();
        }

        public ReferenceCollection WithImport(ImportNode import)
        {
            imports.Add(import);
            return this;
        }

        public ReferenceCollection WithReference(IReferencable referencable)
        {
            foreach (var reference in this)
                if (referencable == reference)
                    return this;

            references.Add(referencable);
            return this;
        }

        public bool HasNotReference(IReferencable referencable)
        {
            foreach (var refer in references)
                if (referencable == refer)
                    return false;

            return true;
        }

        public IEnumerator<IReferencable> GetEnumerator()
        {
            return new GenericEnumeratorService<ReferenceCollection, IReferencable>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
