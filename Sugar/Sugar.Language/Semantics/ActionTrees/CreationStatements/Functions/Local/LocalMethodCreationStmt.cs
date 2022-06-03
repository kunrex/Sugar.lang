using System;

using Sugar.Language.Parsing.Nodes.Functions.Declarations;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Local
{
    internal sealed class LocalMethodCreationStmt : FunctionCreationStmt<FunctionDeclarationNode, IFunctionContainer<LocalMethodCreationStmt>>, IMethodCreation
    {
        public LocalMethodCreationStmt(DataType _creationType, Describer _describer, FunctionArguments arguments, FunctionDeclarationNode _baseNode) : base(
            _creationType,
            _describer,
            DescriberEnum.None,
            arguments,
            _baseNode)
        {

        }

        public override string ToString() => $"Local Function Declaration Node";
    }
}
