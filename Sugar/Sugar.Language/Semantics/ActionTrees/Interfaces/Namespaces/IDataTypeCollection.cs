﻿using System;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.DataTypes;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces.Namespaces
{
    internal interface IDataTypeCollection : IActionTreeNode
    {
        public int DataTypeCount { get; }

        public DataType this[int index] { get; }

        public DataType TryFindDataType(IdentifierNode identifier);
        public IDataTypeCollection AddDataType(DataType datatypeToAdd);
    }
}
