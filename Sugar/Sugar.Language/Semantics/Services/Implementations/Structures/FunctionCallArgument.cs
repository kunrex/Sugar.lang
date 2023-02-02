using System;

using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation.Local.FunctionArguments;

namespace Sugar.Language.Semantics.Services.Implementations.Structures
{
    internal sealed class FunctionCallArgument : IFunctionArgument
    {
        private readonly DataType creationType;
        public DataType CreationType { get => creationType; }

        private readonly Describer describer;
        public Describer Describer { get => describer; }

        public FunctionCallArgument(DataType _creationType, Describer _describer)
        {
            describer = _describer;
            creationType = _creationType;
        }
    }
}
