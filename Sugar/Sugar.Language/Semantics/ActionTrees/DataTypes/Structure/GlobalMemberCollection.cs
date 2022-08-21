using System;
using System.Linq;
using System.Collections.Generic;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.Interfaces;
using Sugar.Language.Semantics.ActionTrees.CreationStatements;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global;

namespace Sugar.Language.Semantics.ActionTrees.DataTypes.Structure
{
    internal sealed class GlobalMemberCollection
    {
        private readonly Dictionary<GlobalMemberEnum, List<ICreationStatement>> collection;

        public GlobalMemberCollection(GlobalMemberEnum categories)
        {
            collection = new Dictionary<GlobalMemberEnum, List<ICreationStatement>>();

            var values = Enum.GetValues(typeof(GlobalMemberEnum)).Cast<GlobalMemberEnum>(); 
            foreach (var value in values)
                if ((value & categories) == value)
                    collection.Add(value, new List<ICreationStatement>());
        }

        public int Count { get => collection.Count; }

        public bool IsDuplicateCreationStatement(string name)
        {
            foreach (var value in collection.Values)
                foreach (var statement in value)
                    if (statement.Name == name)
                        return true;

            return false;
        }

        public T GetCreationStatement<T, Parent>(GlobalMemberEnum memberType, string name) where T : CreationStatement<Parent> where Parent : IActionTreeNode
        {
            if(memberType != GlobalMemberEnum.Indexer)
                foreach (var value in collection[memberType])
                    if (value.Name == name)
                        return (T)value;

            return null;
        }

        public IndexerCreationStmt GetIndexerStatement(DataType type) 
        {
            var name = type.Name;

            foreach (var value in collection[GlobalMemberEnum.Indexer])
                if (value.Name == name)
                    return (IndexerCreationStmt)value;

            return null;
        }

        public void Add(GlobalMemberEnum key, ICreationStatement value)
        {
            if (collection.ContainsKey(key))
                collection[key].Add(value);
        }

        public bool Contains(KeyValuePair<GlobalMemberEnum, ICreationStatement> item)
        {
            var name = item.Value.Name;

            if(collection.ContainsKey(item.Key))
            {
                foreach (var value in collection[item.Key])
                    if (name == value.Name)
                        return true;
            }

            return false;
        }
    }
}
