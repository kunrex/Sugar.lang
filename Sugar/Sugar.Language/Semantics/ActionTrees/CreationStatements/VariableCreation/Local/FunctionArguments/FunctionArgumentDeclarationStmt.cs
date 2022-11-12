using System;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Expressions;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation.Local.FunctionArguments
{
    internal class FunctionArgumentDeclarationStmt : LocalVariableDeclarationStmt, IFunctionArgument
    {
        public virtual ExpressionNode Value { get => null; }

        public override CreationTypeEnum CreationEnumType { get => CreationTypeEnum.FunctionArgument; }

        public FunctionArgumentDeclarationStmt(DataType _creationType, IdentifierNode _creationName, Describer _describer) : base(
            _creationType,
            _creationName,
            _describer)
        {

        }

        public override string ToString() => $"Function Argument";
    }
}
