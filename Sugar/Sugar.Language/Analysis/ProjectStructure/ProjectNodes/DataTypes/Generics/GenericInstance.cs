using System;
using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Parsing.Nodes.DataTypes;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Collections;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Referencing;
using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes.Structure;

namespace Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes.Generics
{
    internal abstract class GenericInstance<BaseType, Skeleton> : DataType where BaseType : TypeWrapper<Skeleton> where Skeleton : DataTypeNode
    {
        private readonly BaseType baseType;
        public BaseType Base { get => baseType; }

        public override Describer Describers {get => baseType.Describers;}
        public override IDataTypeCollection Parent { get => baseType.Parent; }
        public override ReferenceCollection References { get => baseType.References; }
        public override ProjectMemberEnum ProjectMemberType { get => baseType.ProjectMemberType; }

        protected GenericInstance(BaseType _type) : base(_type.Name)
        {
            baseType = _type;
        }
        
        public override void ReferenceParent() { }
        public override void ReferenceParent(IReferencable parent) { }
        public override void SetParent(IDataTypeCollection typeParent) {  }

        public override IReferencable GetParent() { return baseType.GetParent(); }
        public override IReferencable[] GetChildReference(string value) { return baseType.GetChildReference(value); }

        public override IDataTypeCollection AddEntity(DataType dataType)
        {
            children.Add(dataType.Name, dataType);

            return this;
        }
        public override DataType TryFindDataType(string value) { return baseType.TryFindDataType(value); }
        
        public override void ReferenceGeneric(GenericReference generic) { }

        public override bool ValidateDescribers() => baseType.ValidateDescribers();
        public override bool MatchDescriber(Describer toMatch) => baseType.MatchDescriber(toMatch);
    }
}
