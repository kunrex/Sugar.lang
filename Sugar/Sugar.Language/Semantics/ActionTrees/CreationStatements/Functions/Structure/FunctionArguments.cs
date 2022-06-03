using System;
using System.Linq;
using System.Collections.Generic;

using Sugar.Language.Semantics.ActionTrees.DataTypes;

namespace Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Structure
{
    internal sealed class FunctionArguments
    {
        private readonly Dictionary<string, DataType> arguments;

        public int Count
        {
            get => arguments.Count;
        }

        public KeyValuePair<string, DataType> this[int index]
        {
            get => arguments.ElementAt(index);
        }

        public DataType this[string key]
        {
            get
            {
                foreach (var pair in arguments)
                    if (pair.Key == key)
                        return pair.Value;

                return null;
            }

            set => throw new NotImplementedException();
        }

        public FunctionArguments()
        {
            arguments = new Dictionary<string, DataType>();
        }

        public bool Exists(string key) => arguments.ContainsKey(key);

        public void Add(string key, DataType value) => arguments.Add(key, value);
    }
}
