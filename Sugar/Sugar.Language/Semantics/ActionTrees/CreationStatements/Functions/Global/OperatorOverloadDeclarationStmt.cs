﻿using System;

using Sugar.Language.Tokens.Operators;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global
{
    internal sealed class OperatorOverloadDeclarationStmt : GlobalFunctionCreationStmt<IOperatorContainer>
    {
        private readonly Operator operatorToOverload;

        public OperatorOverloadDeclarationStmt(DataType _creationType, IdentifierNode _name, Describer _describer, FunctionArguments _arguments, Node _nodeBody, Operator _operator) : base(
            _creationType,
            _name,
            _describer,
            _arguments,
            _nodeBody)
        {
            operatorToOverload = _operator;
        }

        public override string ToString() => $"Operator Overload Node [Operator: {operatorToOverload.Value}]";
    }
}
