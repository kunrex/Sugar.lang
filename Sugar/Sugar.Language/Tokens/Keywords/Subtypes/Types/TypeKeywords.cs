using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Tokens.Keywords.Subtypes.Types
{
    internal sealed partial class TypeKeyword : Keyword
    {
        public static readonly TypeKeyword Byte = new TypeKeyword("byte", SyntaxKind.Byte);
        public static readonly TypeKeyword SByte = new TypeKeyword("sbyte", SyntaxKind.SByte);

        public static readonly TypeKeyword Short = new TypeKeyword("short", SyntaxKind.Short);
        public static readonly TypeKeyword UShort = new TypeKeyword("ushort", SyntaxKind.UShort);

        public static readonly TypeKeyword Int = new TypeKeyword("int", SyntaxKind.Int);
        public static readonly TypeKeyword UInt = new TypeKeyword("uint", SyntaxKind.UInt);

        public static readonly TypeKeyword Long = new TypeKeyword("long", SyntaxKind.Long);
        public static readonly TypeKeyword Ulong = new TypeKeyword("ulong", SyntaxKind.Ulong);

        public static readonly TypeKeyword Float = new TypeKeyword("float", SyntaxKind.Float);
        public static readonly TypeKeyword Double = new TypeKeyword("double", SyntaxKind.Double);
        public static readonly TypeKeyword Decimal = new TypeKeyword("decimal", SyntaxKind.Decimal);

        public static readonly TypeKeyword Char = new TypeKeyword("char", SyntaxKind.Char);
        public static readonly TypeKeyword String = new TypeKeyword("string", SyntaxKind.String);

        public static readonly TypeKeyword Bool = new TypeKeyword("bool", SyntaxKind.Bool);

        public static readonly TypeKeyword Array = new TypeKeyword("Array", SyntaxKind.Array);
        public static readonly TypeKeyword Object = new TypeKeyword("object", SyntaxKind.Object);
    }
}
