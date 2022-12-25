using System;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global
{
    internal sealed class VoidDeclarationStmt : GlobalVoidDeclarationStmt<MethodDeclarationStmt, VoidDeclarationStmt>, IGlobalFunction
    {
        private readonly IdentifierNode identifier;

        public override ActionNodeEnum ActionNodeType { get => ActionNodeEnum.GlobalVoid; }

        public VoidDeclarationStmt(IdentifierNode _name, Describer _describer, FunctionArguments _arguments, Node _nodeBody) : base(
            _name.Value,
            _describer,
            _arguments,
            _nodeBody)
        {
            identifier = _name;
        }

        public override string ToString() => $"Void Declaration Stmt";
    }
}
