using System;

using Sugar.Language.Parsing.Nodes.DataTypes;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Referencing;

namespace Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes.Generics
{
    internal sealed class GenericReference : IReferencable
    {
        public MemberTypeEnum MemberType { get => MemberTypeEnum.Global; }
        public ProjectMemberEnum ProjectMemberType { get => throw new NotImplementedException(); }

        private readonly string name;
        public string Name { get => name; }

        private readonly InheritanceNode inheritance;
        public InheritanceNode Inheritance { get => inheritance; }
        
        public GenericReference(string _name, InheritanceNode _inheritance)
        {
            name = _name;

            inheritance = _inheritance;
        }

        public IReferencable GetParent() { return null; }
        public IReferencable[] GetChildReference(string value) { return null; }
    }
}
