using System;

using Sugar.Language.Parsing.Nodes;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.Describers;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global
{
    internal abstract class GlobalVoidDeclarationStmt<Function, Void> : VoidCreationStmt<Function, Void> where Function : IMethodCreation where Void : IVoidCreation
    {
        public GlobalVoidDeclarationStmt(string _name, Describer _describer, FunctionDeclArgs _arguments, Node _nodeBody) : base(
            _name,
            _describer,
            DescriberEnum.Static | DescriberEnum.AccessModifiers | DescriberEnum.InheritanceModifiers | DescriberEnum.Override,
            _arguments,
            _nodeBody)
        {

        }
    }
}
