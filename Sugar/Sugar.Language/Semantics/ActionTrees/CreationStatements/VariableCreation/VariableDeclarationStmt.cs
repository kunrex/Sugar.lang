using System;

using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation
{
    internal class VariableDeclarationStmt : VariableCreationStmt
    {
        public VariableDeclarationStmt(DataType _creationType, IdentifierNode _creationName, Describer _describer, bool _isLocal) : base(
            _creationType,
            _creationName,
            _describer,
            _isLocal)
        {
            
        }

        public override string ToString() => $"Variable Declaraion [{creationName.Value}, Local: {isLocal}]";
    }
}
