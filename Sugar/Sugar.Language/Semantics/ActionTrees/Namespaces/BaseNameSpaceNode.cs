using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Interfaces.Collections;

namespace Sugar.Language.Semantics.ActionTrees.Namespaces
{
    internal abstract class BaseNameSpaceNode : ActionTreeNode<INameSpaceCollection>, IDataTypeCollection
    {
        protected readonly List<DataType> dataTypes;

        public int DataTypeCount { get => dataTypes.Count; }

        public override ActionNodeEnum ActionNodeType { get => ActionNodeEnum.Namespace; }

        public BaseNameSpaceNode()  
        {
            dataTypes = new List<DataType>();
        }

        public DataType GetSubDataType(int index) => dataTypes[index];

        public IDataTypeCollection AddEntity(DataType dataType)
        {
            dataTypes.Add(dataType);
            dataType.SetParent(this);

            return this;
        }

        public virtual DataType TryFindDataType(IdentifierNode identifier) 
        {
            foreach (var type in dataTypes)
                if (type.Name == identifier.Value)
                    return type;

            return null;
        }
    }
}
