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
        public IdentifierNode Name { get; private set; }

        protected readonly List<DataType> subTypes;

        protected readonly List<ImportNode> referencedImports;

        protected readonly List<DataType> referencedTypes;
        protected readonly List<CreatedNameSpaceNode> referencedNameSpaces;

        public int DataTypeCount { get => subTypes.Count; }

        public DataType this[int index]
        {
            get => subTypes[index];
        }

        public DataType(IdentifierNode _name, List<ImportNode> _imports)
        {
            Name = _name;

            referencedImports = _imports;
            subTypes = referencedTypes = new List<DataType>();
            referencedNameSpaces = new List<CreatedNameSpaceNode>();
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
            ReferenceSubType(datatypeToAdd);

            return this;
        }

        private void ReferenceSubType(DataType type)
        {
            referencedTypes.Add(type);

            for (int i = 0; i < type.DataTypeCount; i++)
                ReferenceSubType(type[i]);
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

        public DataType FindReferencedType(Node type, DefaultNameSpaceNode defaultNameSpace)
        {
            if(type.NodeType == NodeType.Variable)
            {
                var identifier = (IdentifierNode)type;
                for(int i = 0; i < defaultNameSpace.DataTypeCount; i++)
                {
                    var defaultType = defaultNameSpace[i];

                    if (defaultType.Name.Value == identifier.Value)
                        return defaultType;
                }
            }

            var match = new Queue<IDataTypeCollection>(referencedNameSpaces);
            foreach(var refType in referencedTypes)
                match.Enqueue(refType);

            var current = type;
            while(true)
            {
                switch(current.NodeType)
                {
                    case NodeType.Dot:
                        var dot = (DotExpression)current;

                        var lhs = (IdentifierNode)dot.LHS;
                        var count = match.Count;
                        for (int i = 0; i < count; i++)
                        {
                            switch (match.Dequeue())
                            {
                                case CreatedNameSpaceNode nameSpace:
                                    var nameSpaceResult = nameSpace.TryFindNameSpace(lhs);
                                    if (nameSpaceResult != null)
                                        match.Enqueue(nameSpaceResult);

                                    var result = nameSpace.TryFindDataType(lhs);
                                    if (result != null)
                                        match.Enqueue(result);
                                    break;
                                case DataType dataType:
                                    Console.WriteLine(dataType.Name.Value);
                                    result = dataType.TryFindDataType(lhs);

                                    if (result != null)
                                        match.Enqueue(result);
                                    break;
                            }
                        }

                        current = dot.RHS;
                        break;
                    case NodeType.Variable:
                        var identifier = (IdentifierNode)current;

                        count = match.Count;
                        for (int i = 0; i < count; i++)
                        {
                            switch (match.Dequeue())
                            {
                                case CreatedNameSpaceNode nameSpace:
                                    var nameSpaceResult = nameSpace.TryFindNameSpace(identifier);
                                    if (nameSpaceResult != null)
                                        match.Enqueue(nameSpaceResult);

                                    var result = nameSpace.TryFindDataType(identifier);
                                    if (result != null)
                                        match.Enqueue(result);
                                    break;
                                case DataType dataType:
                                    Console.WriteLine(dataType.Name.Value);
                                    result = dataType.TryFindDataType(identifier);

                                    if (result != null)
                                        match.Enqueue(result);
                                    break;
                            }
                        }

                        switch(match.Count)
                        {
                            case 0:
                                throw new Exception("type referenc doesn't exist");
                            case 1:
                                switch (match.Dequeue())
                                {
                                    case DataType dataType:
                                        return dataType;
                                    default:
                                        throw new Exception("type referenc doesn't exist");
                                }
                            default:
                                throw new Exception("Ambigious reference");
                        }
                }
            }
        }
    }
}
