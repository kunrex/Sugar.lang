using System;
using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Keywords.Subtypes.Entities
{
    internal sealed partial class EntityKeyword : Keyword
    {
        public static readonly EntityKeyword Enum = new EntityKeyword("enum", 0);
        public static readonly EntityKeyword Class = new EntityKeyword("class", 1);
        public static readonly EntityKeyword Struct = new EntityKeyword("struct", 2);
        public static readonly EntityKeyword Interface = new EntityKeyword("interface", 3);
        public static readonly EntityKeyword Namespace = new EntityKeyword("namespace", 4);
    }
}
