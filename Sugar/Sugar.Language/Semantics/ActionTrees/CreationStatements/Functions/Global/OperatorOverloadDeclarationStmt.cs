using System;

using Sugar.Language.Tokens.Operators;

using Sugar.Language.Parsing.Nodes;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;


namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global
{
    internal sealed class OperatorOverloadDeclarationStmt : GlobalMethodCreationStmt<IOperatorContainer>
    {
        private readonly Operator operatorToOverload;
        public Operator Operator { get => operatorToOverload; }

        public override ActionNodeEnum ActionNodeType { get => ActionNodeEnum.OperatorOverload; }

        public OperatorOverloadDeclarationStmt(DataType _creationType, Describer _describer, FunctionDeclArgs _arguments, Node _nodeBody, Operator _operator) : base(
            _creationType,
            _operator.OperatorType.ToString(),
            _describer,
            _arguments,
            _nodeBody)
        {
            operatorToOverload = _operator;
        }

        public override string ToString() => $"Operator Overload Node [Operator: {operatorToOverload.Value}]";
    }
}
