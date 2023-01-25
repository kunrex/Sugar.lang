using System;
using System.Linq;
using System.Collections.Generic;

using Sugar.Language.Semantics.ActionTrees.Enums;
using Sugar.Language.Semantics.ActionTrees.CreationStatements;

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
                var statements = new List<ICreationStatement>();

                ushort i = 1;
                ushort check = (ushort)index, converted = check;
                while (check != 0)
                {
                    if ((converted & i) == i && collection.ContainsKey((MemberEnum)i))
                        foreach (var value in collection[(MemberEnum)i])
                            statements.Add(value);

                    i *= 2;
                    check >>= 1;
                }

                return statements;
            }
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
