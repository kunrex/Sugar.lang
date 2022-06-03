using System;

using Sugar.Language.Tokens.Operators;

using Sugar.Language.Parsing.Nodes.Functions.Declarations.OperatorOverloading;

using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global
{
    internal sealed class OperatorOverloadDeclarationStmt : GlobalFunctionCreationStmt<OperatorOverloadFunctionDeclarationLoad, IOperatorContainer>
    {
        private readonly Operator operatorToOverload;

        public OperatorOverloadDeclarationStmt(DataType _creationType, Describer _describer, FunctionArguments _arguments, OperatorOverloadFunctionDeclarationLoad _baseNode) : base(
            _creationType,
            _describer,
            _arguments,
            _baseNode)
        {
            operatorToOverload = _baseNode.Operator;
        }

        public override string ToString() => $"Operator Overload Node [Operator: {operatorToOverload.Value}]";
    }
}
