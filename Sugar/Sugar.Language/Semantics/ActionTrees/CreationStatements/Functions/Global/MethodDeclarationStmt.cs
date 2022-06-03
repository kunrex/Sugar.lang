using System;

using Sugar.Language.Parsing.Nodes.Functions.Declarations;

using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global
{
    internal class MethodDeclarationStmt : GlobalFunctionCreationStmt<FunctionDeclarationNode, IFunctionContainer<MethodDeclarationStmt>>, IMethodCreation
    {
        public MethodDeclarationStmt(DataType _creationType, Describer _describer, FunctionArguments _arguments, FunctionDeclarationNode _baseNode) : base(
            _creationType,
            _describer,
            _arguments,
            _baseNode)
        {
            
        }

        public override string ToString() => $"Function Declaration Node";
    }
}
