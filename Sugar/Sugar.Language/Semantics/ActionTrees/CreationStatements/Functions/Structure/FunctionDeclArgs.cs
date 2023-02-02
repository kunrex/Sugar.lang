using System;

using Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation.Local.FunctionArguments;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure
{
    internal sealed class FunctionDeclArgs : FunctionArguments<string, FunctionArgumentDeclarationStmt>
    {
        public FunctionDeclArgs()
        {

        }

        public FunctionArgumentDeclarationStmt this[string key] => Arguments[key];
    }
}
