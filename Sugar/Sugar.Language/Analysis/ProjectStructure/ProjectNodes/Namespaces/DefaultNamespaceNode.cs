using System;
using System.Linq;
using System.Collections.Generic;

using Sugar.Language.Analysis.ProjectStructure.Enums;

using Sugar.Language.Analysis.ProjectStructure.Interfaces.Collections;

using Sugar.Language.Analysis.ProjectStructure.ProjectNodes.DataTypes;
using Sugar.Language.Analysis.ProjectStructure.Interfaces.Referencing;

namespace Sugar.Language.Analysis.ProjectStructure.ProjectNodes.Namespaces
{
    internal sealed class DefaultNamespaceNode : BaseNamespaceNode
    {
        private readonly Dictionary<string, DataType> internalTypes;

        public int IntenalTypeCount { get => internalTypes.Count; }

        public override ProjectMemberEnum ProjectMemberType { get => ProjectMemberEnum.DefaultNameSpace; }

        public DefaultNamespaceNode() : base("default")
        {
            internalTypes = new Dictionary<string, DataType>();
        }

        public override void SetParent(INamespaceCollection _parent)
        {
            base.SetParent(_parent);

            foreach (var child in internalTypes.Keys)
                internalTypes[child].SetParent(this);
        }

        public IDataTypeCollection AddInternalDataType(DataType dataType)
        {
            if (!internalTypes.ContainsKey(dataType.Name))
            {
                internalTypes.Add(dataType.Name, dataType);
                dataType.SetParent(this);
            }

            return this;
        }

        public DataType GetInternalDataType(WrapperTypeEnum dataTypeEnum) => internalTypes[dataTypeEnum.ToString()];
        public DataType GetInternalDataType(InternalTypeEnum dataTypeEnum) => GetInternalDataType((WrapperTypeEnum)dataTypeEnum);

        public override DataType TryFindDataType(string value)
        {
            var result = base.TryFindDataType(value);
            if (result != null)
                return result;

            internalTypes.TryGetValue(value, out var val);
            return val;
        }

        public override IReferencable GetParent() { throw new NotImplementedException(); }

        public override IReferencable[] GetChildReference(string value)
        {
            foreach (var child in children)
                if (child.Key == value)
                    return new IReferencable[] { child.Value };

            return null;
        }

        public override string ToString() => $"Default Namespace";

        protected override void PrintChildren(string indent)
        {
            for (int i = 0; i < children.Count; i++)
                children.ElementAt(i).Value.Print(indent, i == children.Count - 1);
        }
    }
}
