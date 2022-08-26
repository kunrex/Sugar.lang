﻿using System;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Expressions;

using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation
{
    internal class GlobalVariableInitialisationStmt : GlobalVariableDeclarationStmt, IInitialisable
    {
        public ExpressionNode Value { get; protected set; }

        protected GlobalVariableInitialisationStmt(DataType _creationType, IdentifierNode _creationName, Describer _describer) : base(
            _creationType,
            _creationName,
            _describer)
        {
            
        }

        public GlobalVariableInitialisationStmt(DataType _creationType, IdentifierNode _creationName, Describer _describer, ExpressionNode _value) : base(
            _creationType,
            _creationName,
            _describer)
        {
            Value = _value;
        }

        public override string ToString() => $"Global Variable Initialisation [{creationName.Value}";
    }
}
