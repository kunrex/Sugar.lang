using System;
using System.Collections.Generic;
using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Interfaces.Namespaces;

namespace Sugar.Language.Semantics.ActionTrees.Namespaces
{
    internal abstract class BaseNameSpaceNode : ActionTreeNode, IDataTypeCollection
    {
        protected readonly List<DataType> dataTypes;

        public BaseNameSpaceNode()  
        {
            dataTypes = new List<DataType>();
        }

        public IDataTypeCollection AddDataType(DataType dataType)
        {
            dataTypes.Add(dataType);

            return this;
        }

        public bool ContainsDataType(DataType dataTypeToCheck)
        {
            foreach (var type in dataTypes)
                if (dataTypeToCheck == type)
                    return true;

            return false;
        }

        public DataType TryFindDataType(IdentifierNode identifier)
        {
            foreach (var type in dataTypes)
                if (type.Name.Value == identifier.Value)
                    return type;

            return null;
        }
    }
}
