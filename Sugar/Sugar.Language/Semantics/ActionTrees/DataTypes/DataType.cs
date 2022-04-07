using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Statements;
using Sugar.Language.Parsing.Nodes.Expressions.Associative;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.Namespaces;
using Sugar.Language.Semantics.ActionTrees.Interfaces.Namespaces;

namespace Sugar.Language.Semantics.ActionTrees.DataTypes
{
    internal abstract class DataType : ParentableActionTreeNode<IDataTypeCollection>, IDataTypeCollection
    {
        public abstract DataTypeEnum TypeEnum { get; }

        protected readonly IdentifierNode name;
        public string Name { get => name.Value; }

        protected readonly List<DataType> subTypes;

        protected readonly List<ImportNode> referencedImports;

        protected readonly List<DataType> referencedTypes;
        protected readonly List<CreatedNameSpaceNode> referencedNameSpaces;

        public int DataTypeCount { get => subTypes.Count; }

        public DataType(IdentifierNode _name, List<ImportNode> _imports)
        {
            name = _name;

            referencedImports = _imports;
            subTypes = referencedTypes = new List<DataType>();
            referencedNameSpaces = new List<CreatedNameSpaceNode>();
        }

        public DataType GetSubDataType(int index) => subTypes[index];

        public DataType TryFindDataType(IdentifierNode identifier)
        {
            foreach (var type in subTypes)
                if (type.Name == identifier.Value)
                    return type;

            return null;
        }

        public IDataTypeCollection AddEntity(DataType datatypeToAdd)
        {
            subTypes.Add(datatypeToAdd);
            ReferenceSubType(datatypeToAdd);

            return this;
        }

        private void ReferenceSubType(DataType type)
        {
            referencedTypes.Add(type);

            for (int i = 0; i < type.DataTypeCount; i++)
                ReferenceSubType(type.GetSubDataType(i));
        }

        public void ReferenceParentNamespaces()
        {
            var parent = (CreatedNameSpaceNode)Parent;

            while (parent != null)
            {
                referencedNameSpaces.Add(parent);
                parent = (CreatedNameSpaceNode)parent.Parent;
            }
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

        public DataType WithReferencedNamespace(CreatedNameSpaceNode createdNameSpaces)
        {
            referencedNameSpaces.Add(createdNameSpaces);

            return this;
        }

        public DataType WithReferencedDataType(DataType dataTypes)
        {
            referencedTypes.Add(dataTypes);

            return this;
        }

        public DataType FindReferencedType(Node type, DefaultNameSpaceNode defaultNameSpace, CreatedNameSpaceCollectionNode collectionNode)
        {
            var dataTypes = new Queue<DataType>(referencedTypes);
            var nameSpaces = new Queue<CreatedNameSpaceNode>(referencedNameSpaces);

            for (int i = 0; i < defaultNameSpace.DataTypeCount; i++)
                dataTypes.Enqueue(defaultNameSpace.GetSubDataType(i));

            var current = type;
            bool first = true;

            while (true)
            {
                switch(current.NodeType)
                {
                    case NodeType.Dot:
                        var dot = (DotExpression)current;

                        Match((IdentifierNode)dot.LHS);
                        current = dot.RHS;
                        break;
                    case NodeType.Variable:
                        Match((IdentifierNode)current);

                        switch (dataTypes.Count)
                        {
                            case 0:
                                throw new Exception("type reference doesn't exist");
                            case 1:
                                return dataTypes.Dequeue();
                            default:
                                throw new Exception("ambigious reference");
                        }
                }

                if (first)
                    first = false;
            }

            void Match(IdentifierNode identifier)
            {
                int nameSpaceCount = nameSpaces.Count, dataTypeCount = dataTypes.Count;
                for (int i = 0; i < nameSpaceCount; i++)
                {
                    var nameSpace = nameSpaces.Dequeue();

                    var nameSpaceResult = nameSpace.TryFindNameSpace(identifier);

                    if (nameSpaceResult != null)
                        nameSpaces.Enqueue(nameSpaceResult);
                    else if(first)
                    {
                        var baseNamespace = collectionNode.TryFindNameSpace(identifier);

                        if (baseNamespace != null && baseNamespace != Parent)
                            nameSpaces.Enqueue(baseNamespace);
                    }

                    TryFindDataType(nameSpace);
                }

                for(int i = 0; i < dataTypeCount; i++)
                    TryFindDataType(dataTypes.Dequeue());

                void TryFindDataType(IDataTypeCollection dataType)
                {
                    var result = dataType.TryFindDataType(identifier);

                    if (result != null)
                        dataTypes.Enqueue(result);
                }
            }
        }
    }
}
