using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Statements;
using Sugar.Language.Parsing.Nodes.Expressions.Associative;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.Services.Interfaces;
using Sugar.Language.Semantics.ActionTrees.Namespaces;
using Sugar.Language.Semantics.ActionTrees.Interfaces.Collections;
using Sugar.Language.Exceptions.Analytics.ClassMemberCreation.SubTypeSearching;

namespace Sugar.Language.Semantics.ActionTrees.DataTypes
{
    internal abstract class DataType : ParentableActionTreeNode<IDataTypeCollection>, IDataTypeCollection, ISubTypeSearcher
    {
        public abstract DataTypeEnum TypeEnum { get; }

        protected readonly IdentifierNode name;
        public string Name { get => name.Value; }

        protected readonly List<DataType> subTypes;
        public int DataTypeCount { get => subTypes.Count; }

        protected readonly List<ImportNode> referencedImports;
        public IEnumerable<ImportNode> ReferencedNameSpaces
        {
            get
            {
                foreach (var nameSpace in referencedImports)
                    yield return nameSpace;
            }
        }

        protected readonly List<DataType> referencedTypes;

        protected readonly List<CreatedNameSpaceNode> referencedNameSpaces;

        public DataType(IdentifierNode _name, List<ImportNode> _imports)
        {
            name = _name;

            referencedImports = _imports;
            subTypes = new List<DataType>();
            referencedTypes = new List<DataType>();
            referencedNameSpaces = new List<CreatedNameSpaceNode>();
        }

        public DataType GetSubDataType(int index) => subTypes[index];

        public DataType TryFindDataType(IdentifierNode identifier)
        {
            var value = identifier.Value;
            foreach (var type in subTypes)
                if (type.Name == value)
                    return type;

            return null;
        }

        public IDataTypeCollection AddEntity(DataType datatypeToAdd)
        {
            subTypes.Add(datatypeToAdd);
            referencedTypes.Add(datatypeToAdd);

            return this;
        }

        public void ReferenceDataType(DataType dataTypes) => referencedTypes.Add(dataTypes);

        public void ReferenceNameSpace(CreatedNameSpaceNode createdNameSpaces) => referencedNameSpaces.Add(createdNameSpaces);

        public void ReferenceParentNameSpaces()
        {
            var parent = (CreatedNameSpaceNode)Parent;

            while (parent != null)
            {
                referencedNameSpaces.Add(parent);
                parent = (CreatedNameSpaceNode)parent.Parent;
            }
        }

        public virtual DataType FindReferencedType(Node type, CreatedNameSpaceCollectionNode collectionNode)
        {
            var dataTypes = new Queue<DataType>(referencedTypes);
            var nameSpaces = new Queue<CreatedNameSpaceNode>(referencedNameSpaces);

            var first = true;
            var current = type;

            while (true)
            {
                switch (current.NodeType)
                {
                    case NodeType.Dot:
                        var dot = (DotExpression)current;

                        Match((IdentifierNode)dot.LHS);
                        current = dot.RHS;
                        break;
                    case NodeType.Variable:
                        var identifier = (IdentifierNode)current;

                        var value = identifier.Value;
                        int dataTypeCount = dataTypes.Count;

                        if (first)
                        {
                            for (int i = 0; i < dataTypeCount; i++)
                            {
                                var dataType = dataTypes.Dequeue();

                                if (dataType.Name == value)
                                    dataTypes.Enqueue(dataType);
                            }
                        }
                        else
                            Match(identifier);

                        switch (dataTypes.Count)
                        {
                            case 0:
                                throw new NoReferenceToTypeException(value, Name, 0);
                            case 1:
                                return dataTypes.Dequeue();
                            default:
                                throw new AmbigiousReferenceException(value, Name, 0);
                        }
                }

                if (first)
                    first = false;
            }

            void Match(IdentifierNode identifier)
            {
                int nameSpaceCount = nameSpaces.Count, dataTypeCount = dataTypes.Count, i = 0;

                for (i = 0; i < nameSpaceCount; i++)
                {
                    var nameSpace = nameSpaces.Dequeue();

                    if(first)
                    {
                        if (nameSpace.Name == identifier.Value)
                            nameSpaces.Enqueue(nameSpace);

                        var baseNamespace = collectionNode.TryFindNameSpace(identifier);
                        if (baseNamespace != null && baseNamespace != Parent)
                        {
                            if (baseNamespace.Name == identifier.Value)
                            {
                                nameSpaces.Enqueue(baseNamespace);
                                continue;
                            }
                        }
                    }

                    var nameSpaceResult = nameSpace.TryFindNameSpace(identifier);
                    if (nameSpaceResult != null)
                        nameSpaces.Enqueue(nameSpaceResult);

                    TryFindDataType(nameSpace);
                }

                if (i == 0 && first)
                {
                    var baseNamespace = collectionNode.TryFindNameSpace(identifier);
                    if (baseNamespace != null && baseNamespace != Parent)
                        nameSpaces.Enqueue(baseNamespace);
                }

                for (i = 0; i < dataTypeCount; i++)
                {
                    var dataType = dataTypes.Dequeue();
                    if(first)
                    {
                        dataTypes.Enqueue(dataType);
                        continue;
                    }

                    TryFindDataType(dataType);
                }

                void TryFindDataType(IDataTypeCollection dataType)
                {
                    var result = dataType.TryFindDataType(identifier);

                    if (result != null)
                        dataTypes.Enqueue(result);
                }

            }
        }

        protected override void PrintChildren(string indent)
        {
            for (int i = 0; i < subTypes.Count; i++)
                subTypes[i].Print(indent, i == subTypes.Count - 1);

            if (referencedImports.Count > 0)
            {
                Console.WriteLine("Referenced Data Types");
                for (int i = 0; i < referencedTypes.Count; i++)
                    referencedTypes[i].Print(indent, i == referencedImports.Count - 1);

                Console.WriteLine("Referenced Name Spaces");
                for (int i = 0; i < referencedNameSpaces.Count; i++)
                    referencedNameSpaces[i].Print(indent, i == referencedImports.Count - 1);
            }
            else
                Console.WriteLine(indent + "Imports: None");
        }
    }
}
