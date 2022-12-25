using System;
using System.Linq;
using System.Collections.Generic;

using Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation.Local.FunctionArguments;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure
{
    internal sealed class FunctionArguments 
    {
        public int Count { get => arguments.Count; }

        private readonly Dictionary<string, FunctionArgumentDeclarationStmt> arguments;

        public FunctionArguments()
        {
            arguments = new Dictionary<string, FunctionArgumentDeclarationStmt>();
        }

        public FunctionArgumentDeclarationStmt this[int key]
        {
            get => arguments.ElementAt(key).Value;
        }

        public FunctionArgumentDeclarationStmt this[string key]
        {
            get => arguments[key];
        }

        public void Add(string key, FunctionArgumentDeclarationStmt value) => arguments.Add(key, value);

        public bool ContainsArgument(string name) => arguments.ContainsKey(name);
    }
}
