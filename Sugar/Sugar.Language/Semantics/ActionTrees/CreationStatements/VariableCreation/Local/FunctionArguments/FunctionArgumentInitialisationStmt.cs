using System;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Expressions;

using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation.Local.FunctionArguments
{
    internal sealed class FunctionArgumentInitialisationStmt : FunctionArgumentDeclarationStmt
    {
        private readonly ExpressionNode value;
        public override ExpressionNode Value { get => value; }

        public FunctionArgumentInitialisationStmt(DataType _creationType, IdentifierNode _creationName, Describer _describer, ExpressionNode _value) : base(
            _creationType,
            _creationName,
            _describer)
        {
            value = _value;
        }

        public override string ToString() => $"Function Argument Initialisation";
    }
}
