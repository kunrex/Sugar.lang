using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes;

using Sugar.Language.Semantics.Services.Interfaces;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Namespaces;
using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Expressions.Associative;
using Sugar.Language.Exceptions.Analytics.ClassMemberCreation.SubTypeSearching;
using Sugar.Language.Semantics.ActionTrees.Interfaces.Collections;

namespace Sugar.Language.Semantics.Services.Implementations
{
    internal sealed class SubTypeSearcherService : ISubTypeSearcherService
    {
        private readonly DataType dataType;

        private readonly DefaultNameSpaceNode defaultNameSpaces;
        private readonly CreatedNameSpaceCollectionNode createdNameSpaces;

        private Queue<DataType> dataTypes { get; set; }
        private Queue<CreatedNameSpaceNode> nameSpaces { get; set; }

        public SubTypeSearcherService(DataType _dataType, DefaultNameSpaceNode _defaultNameSpaces, CreatedNameSpaceCollectionNode _createdNameSpaces)
        {
            dataType = _dataType;

            defaultNameSpaces = _defaultNameSpaces;
            createdNameSpaces = _createdNameSpaces;

            dataTypes = new Queue<DataType>();
            nameSpaces = new Queue<CreatedNameSpaceNode>();
        }

        public DataType TryFindReferencedType(Node type)
        {
            dataTypes.Clear();
            foreach(var item in dataType.ReferencedTypes)
                dataTypes.Enqueue(item);

            for(int i = 0; i < defaultNameSpaces.DataTypeCount; i++)
                dataTypes.Enqueue(defaultNameSpaces.GetSubDataType(i));

            nameSpaces.Clear();
            foreach (var item in dataType.ReferncedNameSpaces)
                nameSpaces.Enqueue(item);

            switch(type.NodeType)
            {
                case NodeType.Dot:
                    var dot = (DotExpression)type;

                    Match((IdentifierNode)dot.LHS);
                    type = dot.RHS;
                    break;
                case NodeType.Variable:
                    var value = ((IdentifierNode)type).Value;

                    for (int i = 0; i < dataTypes.Count; i++)
                    {
                        var dataType = dataTypes.Dequeue();

                        if (dataType.Name == value)
                            dataTypes.Enqueue(dataType);
                    }

                    return TryMatchDataType(value);
            }

            return FindReferencedType(type);

            void Match(IdentifierNode identifier)
            {
                int nameSpaceCount = nameSpaces.Count, dataTypeCount = dataTypes.Count;

                for (int i = 0; i < nameSpaceCount; i++)
                {
                    var nameSpace = nameSpaces.Dequeue();

                    if (nameSpace.Name == identifier.Value)
                        nameSpaces.Enqueue(nameSpace);

                    TryFindNameSpace(nameSpace, identifier);
                }

                for(int i = 0; i < dataTypeCount; i++)
                {
                    var dataType = dataTypes.Dequeue();

                    if (dataType.Name == identifier.Value)
                        dataTypes.Enqueue(dataType);

                    TryFindDataType(dataType, identifier);
                }

                TryFindBaseNameSpace(identifier);
            }
        }

        private DataType FindReferencedType(Node type)
        {
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

                        Match(identifier);
                        return TryMatchDataType(identifier.Value);
                }
            }

            void Match(IdentifierNode identifier)
            {
                int nameSpaceCount = nameSpaces.Count, dataTypeCount = dataTypes.Count;

                for (int i = 0; i < nameSpaceCount; i++)
                    TryFindNameSpace(nameSpaces.Dequeue(), identifier);

                TryFindDataType(dataTypeCount, identifier);
            }
        }

        private void TryFindBaseNameSpace(IdentifierNode identifier)
        {
            var baseNamespace = createdNameSpaces.TryFindNameSpace(identifier);

            if (baseNamespace != dataType.Parent && baseNamespace.Name == identifier.Value)
                nameSpaces.Enqueue(baseNamespace);
        }

        private void TryFindNameSpace(CreatedNameSpaceNode nameSpace, IdentifierNode identifier)
        {
            var nameSpaceResult = nameSpace.TryFindNameSpace(identifier);
            if (nameSpaceResult != null)
                nameSpaces.Enqueue(nameSpaceResult);

            TryFindDataType(nameSpace, identifier);
        }

        private void TryFindDataType(int dataTypeCount, IdentifierNode identifier)
        {
            for (int i = 0; i < dataTypeCount; i++)
                TryFindDataType(dataTypes.Dequeue(), identifier);
        }

        private void TryFindDataType(IDataTypeCollection dataType, IdentifierNode identifier)
        {
            var result = dataType.TryFindDataType(identifier);

            if (result != null)
                dataTypes.Enqueue(result);
        }

        private DataType TryMatchDataType(string value)
        {
            switch (dataTypes.Count)
            {
                case 0:
                    throw new NoReferenceToTypeException(value, dataType.Name, 0);
                case 1:
                    return dataTypes.Dequeue();
                default:
                    throw new AmbigiousReferenceException(value, dataType.Name, 0);
            }
        }
    }
}
