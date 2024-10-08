﻿using System;

using Sugar.Language.Exceptions.Analysis.Processing;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Parenting;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Collections;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Referencing;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;

namespace Sugar.Language.Analysis.ProjectStructure.ProjectNodes.Namespaces
{
    internal abstract class BaseNamespaceNode : CollectionProjectNode<DataType>, IDataTypeCollection, IParentableNode<INamespaceCollection>
    {
        protected INamespaceCollection parent;
        public INamespaceCollection Parent { get => parent; }

        public int DataTypeCount { get => Length; }
        public override DataType this[int index] { get => throw new NotImplementedException(); }

        public BaseNamespaceNode(string _name) : base(_name) { }

        public virtual void SetParent(INamespaceCollection namespaceParent)
        {
            if (parent != null)
                throw new DoubleParentAssignementException();

            parent = namespaceParent;

            foreach (var child in children)
                child.Value.SetParent(this);
        }

        public IDataTypeCollection AddEntity(DataType dataType)
        {
            children.Add(dataType.Name, dataType);

            return this;
        }

        public virtual DataType TryFindDataType(string value)
        {
            children.TryGetValue(value, out var val);
            return val;
        }

        public abstract IReferencable GetParent();

        public abstract IReferencable[] GetChildReference(string value);
    }
}
 
