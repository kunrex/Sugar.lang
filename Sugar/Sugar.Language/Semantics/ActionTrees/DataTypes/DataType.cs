using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Statements;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.Interfaces.Namespaces;

namespace Sugar.Language.Semantics.ActionTrees.DataTypes
{
    internal abstract class DataType : ActionTreeNode, IDataTypeCollection
    {
        public abstract DataTypeEnum TypeEnum { get; }
        public IdentifierNode Name { get; private set; }

        protected readonly List<DataType> subTypes;

        protected readonly List<ImportNode> referencedImports;

        public int DatatypeCount { get => subTypes.Count; }

        public DataType this[int index]
        {
            get => subTypes[index];
        }

        public DataType(IdentifierNode _name, List<ImportNode> _imports)
        {
            Name = _name;

            referencedImports = _imports;
            subTypes = new List<DataType>();
        }

        public DataType TryFindDataType(IdentifierNode identifier)
        {
            foreach (var type in subTypes)
                if (type.Name.Value == identifier.Value)
                    return type;

            return null;
        }

        public IDataTypeCollection AddDataType(DataType datatypeToAdd)
        {
            subTypes.Add(datatypeToAdd);

            return this;
        }

        protected override void PrintChildren(string indent)
        {
            for (int i = 0; i < subTypes.Count; i++)
                subTypes[i].Print(indent, i == subTypes.Count - 1);

            if (referencedImports.Count > 0)
            {
                Console.WriteLine(indent + "Imports: ");

                for (int i = 0; i < referencedImports.Count; i++)
                    referencedImports[i].Print(indent, i == referencedImports.Count - 1);
            }
            else
                Console.WriteLine(indent + "Imports: None");
        }

        public IEnumerable<ImportNode> GetReferencedNamespaces()
        {
            foreach (var import in referencedImports)
                yield return import;
        }
    }
}
