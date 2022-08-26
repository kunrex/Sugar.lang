using System;
using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.Describers;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation.Global
{
    internal sealed class EnumConstantDeclarationStmt : GlobalVariableInitialisationStmt
    {
        public EnumConstantDeclarationStmt(IdentifierNode _creationName, Describer _describer) : base(
            null,//DefaultDataType.Integer
            _creationName,
            _describer)
        {

        }
    }
}
