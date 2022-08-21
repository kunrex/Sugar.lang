using System;
using System.Collections.Generic;

using Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation.Local.FunctionArguments;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure
{
    internal sealed class FunctionArguments 
    {
        public int Count { get => arguments.Count; }

        private readonly Dictionary<string, IFunctionArgument> arguments;

        public FunctionArguments()
        {
            arguments = new Dictionary<string, IFunctionArgument>();
        }

        public IFunctionArgument this[string key]
        {
            get => arguments[key];
        }

        public void Add(string key, IFunctionArgument value) => arguments.Add(key, value);

        public bool ContainsArgument(string name) => arguments.ContainsKey(name);
    }
}
