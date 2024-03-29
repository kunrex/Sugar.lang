﻿using System;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global
{
    internal sealed class MethodDeclarationStmt : GlobalMethodCreationStmt<IFunctionContainer<MethodDeclarationStmt, VoidDeclarationStmt>>, IGlobalFunction, IMethodCreation
    {
        private readonly IdentifierNode identifier;

        public override ActionNodeEnum ActionNodeType { get => ActionNodeEnum.GlobalFunction; }

        public MethodDeclarationStmt(DataType _creationType, IdentifierNode _name, Describer _describer, FunctionDeclArgs _arguments, Node _nodeBody) : base(
            _creationType,
            _name.Value,
            _describer,
            _arguments,
            _nodeBody)
        {
            identifier = _name;
        }

        public override string ToString() => $"Function Declaration Node";
    }
}
