using System;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.PropertyCreation.PropertyIdentifiers;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.PropertyCreation
{
    internal sealed class PropertyInitialisationStmt : PropertyDeclarationStmt, IInitialisableCreationStatement 
    {
        public Node Value { get; private set; }

        public PropertyInitialisationStmt(DataType _creationType, IdentifierNode _creationName, Describer _describer, PropertyGetIdentifier _get, PropertySetIdentifier _set, Node _value) : base(
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
