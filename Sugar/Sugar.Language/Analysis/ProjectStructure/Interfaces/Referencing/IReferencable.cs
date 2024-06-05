using System;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Nodes;

namespace Sugar.Language.Analysis.ProjectStructure.Interfaces.Referencing
{
    internal interface IReferencable: IProjectNode, INameable
    {
        public IReferencable GetParent();

        public IReferencable[] GetChildReference(string value);
    }
}
