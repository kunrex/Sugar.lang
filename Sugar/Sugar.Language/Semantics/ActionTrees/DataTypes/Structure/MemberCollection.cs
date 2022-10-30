using System;
using System.Linq;
using System.Collections.Generic;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.Interfaces;
using Sugar.Language.Semantics.ActionTrees.CreationStatements;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global;
using Sugar.Language.Semantics.ActionTrees.CreationStatements.Functions.Global.Conversion;

namespace Sugar.Language.Semantics.ActionTrees.DataTypes.Structure
{
    internal sealed class MemberCollection
    {
        private readonly Dictionary<MemberEnum, List<ICreationStatement>> collection;

        public MemberCollection(MemberEnum categories)
        {
            collection = new Dictionary<MemberEnum, List<ICreationStatement>>();

            var values = Enum.GetValues(typeof(MemberEnum)).Cast<MemberEnum>(); 
            foreach (var value in values)
                if ((value & categories) == value)
                    collection.Add(value, new List<ICreationStatement>());
        }

        public int Count { get => collection.Count; }

        public IEnumerable<ICreationStatement> this[MemberEnum index]
        {
            get
            {
                ushort i = 1;
                ushort check = (ushort)index, converted = check;
                while (index != 0)
                {
                    if ((converted & i) == i && collection.ContainsKey((MemberEnum)i))
                        foreach (var value in collection[index])
                            yield return value;
                    else
                        yield return null;

                    i *= 2;
                    check >>= 1;
                }
            }
        }

        public bool IsDuplicateCreationStatement(string name)
        {
            foreach (var value in collection.Values)
                foreach (var statement in value)
                    if (statement.Name == name)
                        return true;

            return false;
        }

        public T GetCreationStatement<T, Parent>(MemberEnum memberType, string name) where T : CreationStatement<Parent> where Parent : IActionTreeNode
        {
            if ((memberType & MemberEnum.DataTypeMembers) != memberType)
            {
                foreach (var value in collection[memberType])
                    if (value.Name == name)
                        return (T)value;
            }

            return null;
        }

        public IndexerCreationStmt GetIndexerStatement(DataType type) 
        {
            var name = type.Name;

            foreach (var value in collection[MemberEnum.Indexer])
                if (value.Name == name)
                    return (IndexerCreationStmt)value;

            return null;
        }

        public ExplicitCastDeclarationStmt GetExplicitDeclaration(DataType type)
        {
            var name = type.Name;

            foreach (var value in collection[MemberEnum.Indexer])
                if (value.Name == name)
                    return (ExplicitCastDeclarationStmt)value;

            return null;
        }

        public ImplicitCastDeclarationStmt GetImplicitDeclaration(DataType type)
        {
            var name = type.Name;

            foreach (var value in collection[MemberEnum.Indexer])
                if (value.Name == name)
                    return (ImplicitCastDeclarationStmt)value;

            return null;
        }

        public void Add(MemberEnum key, ICreationStatement value)
        {
            if (collection.ContainsKey(key))
                collection[key].Add(value);
        }

        public bool Contains(KeyValuePair<MemberEnum, ICreationStatement> item)
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
