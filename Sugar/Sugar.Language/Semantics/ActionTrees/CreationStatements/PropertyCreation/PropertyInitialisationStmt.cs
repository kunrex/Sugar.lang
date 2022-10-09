using System;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Expressions;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.PropertyCreation
{
    internal sealed class PropertyInitialisationStmt : PropertyDeclarationStmt, IInitialisable 
    {
        public ExpressionNode Value { get; private set; }

        public PropertyInitialisationStmt(DataType _creationType, IdentifierNode _creationName, Describer _describer, Node _get, Node _set, ExpressionNode _value) : base(
            _creationType,
            _creationName,
            _describer,
            _get,
            _set)
        {
            Value = _value;
        }

        public override string ToString() => $"Property Initialisation Node [{creationName}, Type: {PropertyType}]";
    }
}
