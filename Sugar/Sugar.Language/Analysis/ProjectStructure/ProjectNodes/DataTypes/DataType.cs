using System;
using System.Collections.Generic;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Describers;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Collections;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Referencing;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes.Generics;
using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes.Structure;

namespace Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes
{
    internal abstract class DataType : CollectionProjectNode<DataType>, IDataTypeCollection, IParentableNode<IDataTypeCollection>, IDescribable
    {
        public abstract IDataTypeCollection Parent { get; }
        
        public abstract Describer Describers { get; }
        public abstract ReferenceCollection References { get; }

        public int DataTypeCount { get => Length; }
        public override DataType this[int index] { get => children[index]; }

        protected int genericCount;
        public int GenericCount { get => genericCount; }

        protected DataType(string _name) : base(_name)
        {
            genericCount = 0;
        }
        
        public abstract void ReferenceParent();
        public abstract void ReferenceParent(IReferencable parent);
        
        public abstract IReferencable GetParent();
        public abstract void SetParent(IDataTypeCollection typeParent);

        public abstract DataType TryFindDataType(string value);
        public abstract IReferencable[] GetChildReference(string value);
        public abstract IDataTypeCollection AddEntity(DataType dataType);
        
        public abstract bool ValidateDescribers();
        public abstract bool MatchDescriber(Describer toMatch);

        public abstract void ReferenceGeneric(GenericReference generic);
    }
}
