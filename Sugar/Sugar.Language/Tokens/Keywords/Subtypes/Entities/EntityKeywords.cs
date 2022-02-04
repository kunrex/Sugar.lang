using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Keywords.Subtypes.Entities
{
    internal sealed partial class EntityKeyword : Keyword
    {
        public static readonly EntityKeyword Enum = new EntityKeyword("enum", SyntaxKind.Enum);
        public static readonly EntityKeyword Class = new EntityKeyword("class", SyntaxKind.Class);
        public static readonly EntityKeyword Struct = new EntityKeyword("struct", SyntaxKind.Struct);
        public static readonly EntityKeyword Interface = new EntityKeyword("interface", SyntaxKind.Interface);
        public static readonly EntityKeyword Namespace = new EntityKeyword("namespace", SyntaxKind.Namespace);
    }
}
