using System;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Values;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global
{
    internal class GlobalVoidDeclarationStmt : VoidDeclarationStmt<MethodDeclarationStmt, GlobalVoidDeclarationStmt>
    {
        public GlobalVoidDeclarationStmt(IdentifierNode _name, Describer _describer, FunctionArguments _arguments, Node _nodeBody) : base(
            _name,
            _describer,
            DescriberEnum.Static | DescriberEnum.AccessModifiers | DescriberEnum.InheritanceModifiers | DescriberEnum.Override,
            _arguments,
            _nodeBody)
        {
        }


        public override string ToString() => "Void Returnable Statement";
    }
}
