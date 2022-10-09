﻿using System;

using Sugar.Language.Parsing.Nodes;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions
{
    internal abstract class VoidCreationStmt<Function, Void> : CreationStatement<IFunctionContainer<Function, Void>>, IVoidCreation where Function : IMethodCreation where Void : IVoidCreation
    {
        protected readonly Node nodeBody;
        public Node NodeBody { get => nodeBody; }

        protected readonly FunctionArguments arguments;
        public FunctionArguments FunctionArguments { get => arguments; }

        public VoidCreationStmt(string _name, Describer _describer, DescriberEnum _allowed, FunctionArguments _arguments, Node _nodeBody) : base(
            _name,
            _describer,
            _allowed)
        {
            nodeBody = _nodeBody;
            arguments = _arguments;
        }
    }
}