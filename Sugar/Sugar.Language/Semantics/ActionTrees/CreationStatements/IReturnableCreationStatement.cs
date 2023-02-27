using System;

using Sugar.Language.Semantics.ActionTrees.DataTypes;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements
{
    internal interface IReturnableCreationStatement : ICreationStatement
    {
        public DataType CreationType { get; }
    }
}
