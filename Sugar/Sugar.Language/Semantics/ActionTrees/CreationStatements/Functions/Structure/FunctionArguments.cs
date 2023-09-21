using System;
using System.Linq;
using System.Collections.Generic;

using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Interfaces;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.VariableCreation.Local.FunctionArguments;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure
{
    internal abstract class FunctionArguments<Key, ArgumentBase> : IFunctionArguments where ArgumentBase : IFunctionArgument
    {
        public int Count { get => Arguments.Count; }

        protected Dictionary<Key, ArgumentBase> Arguments { get; private set; }

        public DataType this[int key]
        {
            get
            {
                if (key >= 0 && key < Arguments.Count)
                    return Arguments.ElementAt(key).Value.CreationType;

                return null;
            }
        }

        public FunctionArguments()
        {
            Arguments = new Dictionary<Key, ArgumentBase>();
        }

        public bool ContainsArgument(Key name) => Arguments.ContainsKey(name);

        public void Add(Key key, ArgumentBase value) => Arguments.Add(key, value);
    }
}
