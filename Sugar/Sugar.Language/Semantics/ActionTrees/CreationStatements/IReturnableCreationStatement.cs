using System;

using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Interfaces;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements
{
    internal interface IReturnableCreationStatement : IActionTreeNode
    {
        public DataType CreationType { get; }
    }
}
