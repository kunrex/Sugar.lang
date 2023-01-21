using System;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation.Local
{
    internal class LocalVariableInitialisationStmt : LocalVariableDeclarationStmt, IInitialisable
    {
        private readonly Node value;
        public Node Value { get => value; }

        public LocalVariableInitialisationStmt(DataType _creationType, IdentifierNode _creationName, Describer _describer, Node _value) : base(
            _creationType,
            _creationName,
            _describer)
        {
            value = _value;
        }

        public override string ToString() => $"Local Variable Initialisation [{creationName}";
    }
}
