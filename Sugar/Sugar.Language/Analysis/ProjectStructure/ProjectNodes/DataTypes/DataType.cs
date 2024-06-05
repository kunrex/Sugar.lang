using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Exceptions.Analysis.Processing;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Describers;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Collections;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Referencing;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes.Structure;

namespace Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes
{
    internal abstract class DataType : CollectionProjectNode<DataType>, IDataTypeCollection, IParentableNode<IDataTypeCollection>, IDescribable, IReferencable
    {
        private IDataTypeCollection parent;
        public IDataTypeCollection Parent { get => Parent; }

        public int DataTypeCount { get => Length; }

        protected readonly Describer describer;
        public Describer Describers { get => describer; }

        public override DataType this[int index] { get => children[index]; }


        private readonly ReferenceCollection references;
        public ReferenceCollection References { get => references; }

        public DataType(string _name, Describer _describer, ReferenceCollection _references) : base(_name)
        {
            describer = _describer;
            references = new ReferenceCollection(_references);
        }

        public void SetParent(IDataTypeCollection typeParent)
        {
            if (parent != null)
                throw new DoubleParentAssignementException();

            parent = typeParent;

            foreach (var child in children)
                child.SetParent(this);
        }

        public IDataTypeCollection AddEntity(DataType dataType)
        {
            if (SearchChildren(dataType.Name) != null)
                children.Add(dataType);

            return this;
        }

        public DataType TryFindDataType(IdentifierNode identifier) => SearchChildren(identifier.Value);

        private DataType SearchChildren(string value)
        {
            foreach (var type in children)
                if (type.Name == value)
                    return type;

            return null;
        }

        public bool MatchDescriber(Describer tomatch) => tomatch.ValidateDescriber(describer);
        public bool ValidateDescribers() => describer.ValidateDescription(DescriberEnum.AccessModifiers);

        public IReferencable GetParent() { return parent; }

        public void ReferenceParent()
        {
            foreach (var child in children)
            {
                child.ReferenceParent(this);
                child.ReferenceParent();
            }
        }

        public void ReferenceParent(IReferencable parent)
        {
            references.WithReference(parent);

            foreach (var child in children)
                child.ReferenceParent(parent);
        }

        public IReferencable[] GetChildReference(string value)
        {
            foreach (var child in children)
                if (child.Name == value)
                    return new IReferencable[] { child };

            return null;
        }
    }
}
