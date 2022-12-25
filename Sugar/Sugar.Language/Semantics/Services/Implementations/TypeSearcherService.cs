using System;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes;
using Sugar.Language.Parsing.Nodes.Enums;
using Sugar.Language.Parsing.Nodes.Values;
using Sugar.Language.Parsing.Nodes.Expressions.Associative;

using Sugar.Language.Semantics.ActionTrees;
using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.Services.Structure;
using Sugar.Language.Semantics.Services.Interfaces;
using Sugar.Language.Semantics.ActionTrees.DataTypes;
using Sugar.Language.Semantics.ActionTrees.Namespaces;
using Sugar.Language.Semantics.ActionTrees.Interfaces.Collections;

namespace Sugar.Language.Semantics.Services.Implementations
{
    internal sealed class TypeSearcherService : ITypeSearcherService
    {
        private readonly DataType dataType;

        private readonly DefaultNameSpaceNode defaultNameSpace;

        private readonly Queue<DataType> dataTypes;
        private readonly Queue<CreatedNameSpaceNode> nameSpaces;

        public TypeSearcherService(DataType _dataType, DefaultNameSpaceNode _defaultNameSpaces)
        {
            dataType = _dataType;

            defaultNameSpace = _defaultNameSpaces;

            dataTypes = new Queue<DataType>();
            nameSpaces = new Queue<CreatedNameSpaceNode>();
        }

        //should work, just check the algo once pls
        //also maybe clean up lines 50 => 62. in the sense if theres a way to reduce the multiple ifs and foreachs then implement that, not nececarry tho :)
        public Queue<SearchResult<IDataTypeCollection>> ReferenceDataTypeCollection(Node type)
        {
            IdentifierNode immediate;
            if (type.NodeType == NodeType.Variable)
                immediate = (IdentifierNode)type;
            else
                immediate = (IdentifierNode)((DotExpression)type).LHS;

            //check for default data type as a parent
            var nameSpace = CheckParent<DataType, CreatedNameSpaceNode, IDataTypeCollection>(dataType, ActionNodeEnum.Namespace, TryFindDataType);
            var baseNode = CheckParent<CreatedNameSpaceNode, CreatedNameSpaceCollectionNode, INameSpaceCollection>(nameSpace, ActionNodeEnum.NameSpaceCollection, TryFindNameSpace);

            var baseNameSpace = baseNode.TryFindNameSpace(immediate);
            if (baseNameSpace != null)
                nameSpaces.Enqueue(baseNameSpace);

            var baseDataType = defaultNameSpace.TryFindDataType(immediate);
            if (baseDataType != null)
                dataTypes.Enqueue(baseDataType);

            if (immediate.Value == dataType.Name)
                dataTypes.Enqueue(dataType);

            foreach (var referenced in dataType.ReferncedNameSpaces)
                TryFindNameSpace(referenced, immediate);
            foreach (var referenced in dataType.ReferencedTypes)
                TryFindDataType(referenced, immediate);

            if(type.NodeType == NodeType.Variable)
                return MatchReference(type);
            else
                return MatchReference(((DotExpression)type).RHS);

            Parent CheckParent<Type, Parent, ParentInterface>(Type start, ActionNodeEnum parentType, Action<Type, IdentifierNode> function) where Type : ActionTreeNode<ParentInterface>, ParentInterface, INameable where Parent : ParentInterface where ParentInterface : IEntityCollection<Type, ParentInterface>
            {
                ParentInterface parent = start.Parent;

                while(true)
                {
                    if (parent.ActionNodeType == parentType)
                        return (Parent)parent;
                    else
                    {
                        var converted = (Type)parent;
                        function.Invoke(converted, immediate);
                        parent = (ParentInterface)converted.Parent;
                    }
                }
            }
        }

        private Queue<SearchResult<IDataTypeCollection>> MatchReference(Node start)
        {
            var results = new Queue<SearchResult<IDataTypeCollection>>();

            while (true)
            {
                switch(start.NodeType)
                {
                    case NodeType.Dot:
                        var dot = (DotExpression)start;
                        var rhs = (IdentifierNode)dot.RHS;

                        switch(rhs.NodeType)
                        {
                            case NodeType.Variable:
                                Match(rhs, dot.RHS);
                                start = dot.LHS;
                                break;
                            case NodeType.FunctionCall:
                                OnFunctionCall();
                                return results;
                        }
                        break;
                    case NodeType.Variable:
                        var identifier = (IdentifierNode)start;
                        Match(identifier, null);
                        return results;
                    case NodeType.FunctionCall:
                        OnFunctionCall();
                        return results;
                }

                if (nameSpaces.Count == dataTypes.Count && dataTypes.Count == 0)
                    break;

                void Match(IdentifierNode identifier, Node remaining)
                {
                    MatchCollection(nameSpaces, TryRefSubNameSpace);
                    MatchCollection(dataTypes, TryRefSubType);

                    void MatchCollection<DataTypCollection>(Queue<DataTypCollection> queue, Func<DataTypCollection, IdentifierNode, bool> func) where DataTypCollection : IDataTypeCollection
                    {
                        int count = queue.Count;
                        for (int i = 0; i < count; i++)
                        {
                            var collection = queue.Dequeue();
                            var result = func.Invoke(collection, identifier);

                            if (!result)
                                results.Enqueue(new SearchResult<IDataTypeCollection>(collection, remaining));
                        }
                    }
                }

                void OnFunctionCall()
                {
                    EnqeueCollection(dataTypes, start);
                    EnqeueCollection(nameSpaces, start);

                    void EnqeueCollection<DataTypCollection>(Queue<DataTypCollection> queue, Node remaining) where DataTypCollection : IDataTypeCollection
                    {
                        int count = queue.Count;
                        for (int i = 0; i < count; i++)
                        {
                            var collection = queue.Dequeue();
                            results.Enqueue(new SearchResult<IDataTypeCollection>(collection, remaining));
                        }

                        queue.Clear();
                    }
                }
            }

            return results;
        }

        private void TryFindNameSpace(CreatedNameSpaceNode nameSpace, IdentifierNode identifier)
        {
            if (nameSpace.Name == identifier.Value)
                nameSpaces.Enqueue(nameSpace);
            else
                TryRefSubNameSpace(nameSpace, identifier);
        }

        private bool TryRefSubNameSpace(CreatedNameSpaceNode nameSpace, IdentifierNode identifier)
        {
            var nameSpaceResult = nameSpace.TryFindNameSpace(identifier);
            bool result = false;
            if (nameSpaceResult != null)
            {
                nameSpaces.Enqueue(nameSpaceResult);
                result = true;
            }

            return result | TryRefSubType(nameSpace, identifier);
        }

        private void TryFindDataType(DataType dataType, IdentifierNode identifier)
        {
            if (dataType.Name == identifier.Value)
                dataTypes.Enqueue(dataType);
            else
                TryRefSubType(dataType, identifier);
        }

        private bool TryRefSubType(IDataTypeCollection dataType, IdentifierNode identifier)
        {
            var result = dataType.TryFindDataType(identifier);

            if (result != null)
                dataTypes.Enqueue(result);

            return result != null;
        }
    }
}
