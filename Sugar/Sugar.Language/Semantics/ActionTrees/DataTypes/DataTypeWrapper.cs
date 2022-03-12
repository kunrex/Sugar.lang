using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Statements;
using Sugar.Language.Parsing.Nodes.UDDataTypes;

namespace Sugar.Language.Semantics.ActionTrees.DataTypes
{
    internal abstract class DataTypeWrapper<T> : DataType where T : UDDataTypeNode
    {
        public T Skeleton { get; protected set; }

        public DataTypeWrapper(IdentifierNode _name, List<ImportNode> _imports, T _skeleton) : base(_name, _imports)
        {
            Skeleton = _skeleton;
        }
    }
}
