using System;

using Sugar.Language.Exceptions.Analysis.Processing;

using Sugar.Language.Parsing.Nodes.Values;

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
                child.SetParent(this);
        }

        public IDataTypeCollection AddEntity(DataType dataType)
        {
            children.Add(dataType);

            return this;
        }

        public virtual DataType TryFindDataType(string value)
        {
            foreach (var type in children)
                if (type.Name == value)
                    return type;

            return null;
        }

        public abstract IReferencable GetParent();

        public abstract IReferencable[] GetChildReference(string value);
    }
}
 
