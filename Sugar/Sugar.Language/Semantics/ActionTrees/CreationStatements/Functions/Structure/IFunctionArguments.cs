using System;
using System.Collections.Generic;

using Sugar.Language.Semantics.ActionTrees.DataTypes;

namespace Sugar.Language.Semantics.ActionTrees.Interfaces
{
    internal interface IFunctionArguments
    {
        public int Count { get; }

        public DataType this[int ket] { get; }
    }
}
