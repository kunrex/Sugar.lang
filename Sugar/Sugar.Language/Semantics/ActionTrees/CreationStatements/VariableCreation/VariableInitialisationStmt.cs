using System;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Expressions;

using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation
{
    internal sealed class VariableInitialisationStmt : VariableDeclarationStmt, IInitialisable
    {
        public ExpressionNode Value { get; private set; }

        public VariableInitialisationStmt(DataType _creationType, IdentifierNode _creationName, Describer _describer, bool _isLocal, ExpressionNode _value) : base(
            _creationType,
            _creationName,
            _describer,
            _isLocal)
        {
            Value = _value;
        }

        public override string ToString() => $"Variable Initialisation [{creationName.Value}, Local: {isLocal}]";
    }
}
