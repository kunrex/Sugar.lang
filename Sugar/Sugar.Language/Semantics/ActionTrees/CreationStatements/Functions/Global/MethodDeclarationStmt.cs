using System;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.Interfaces.DataTypes;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global
{
    internal class MethodDeclarationStmt : GlobalFunctionCreationStmt<IFunctionContainer<MethodDeclarationStmt, GlobalVoidDeclarationStmt>>, IMethodCreation
    {
        public MethodDeclarationStmt(DataType _creationType, IdentifierNode _name, Describer _describer, FunctionArguments _arguments, Node _nodeBody) : base(
            _creationType,
            _name,
            _describer,
            _arguments,
            _nodeBody)
        {
            
        }

        public override string ToString() => $"Function Declaration Node";
    }
}
