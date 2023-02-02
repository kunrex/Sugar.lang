using System;

using Sugar.Language.Semantics.ActionTrees.DataTypes;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation.Local.FunctionArguments
{
    internal interface IFunctionArgument 
    {
        public DataType CreationType { get; }
    }
}
