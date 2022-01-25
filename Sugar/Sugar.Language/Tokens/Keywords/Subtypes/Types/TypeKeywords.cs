using System;

namespace Sugar.Language.Tokens.Keywords.Subtypes.Types
{
    internal sealed partial class TypeKeyword : Keyword
    {
        public static readonly TypeKeyword Byte = new TypeKeyword("byte", 0);
        public static readonly TypeKeyword SByte = new TypeKeyword("sbyte", 1);

        public static readonly TypeKeyword Short = new TypeKeyword("short", 2);
        public static readonly TypeKeyword UShort = new TypeKeyword("ushort", 3);

        public static readonly TypeKeyword Int = new TypeKeyword("int", 4);
        public static readonly TypeKeyword UInt = new TypeKeyword("uint", 5);

        public static readonly TypeKeyword Long = new TypeKeyword("long", 6);
        public static readonly TypeKeyword Ulong = new TypeKeyword("ulong", 7);

        public static readonly TypeKeyword Float = new TypeKeyword("float", 8);
        public static readonly TypeKeyword Double = new TypeKeyword("double", 9);
        public static readonly TypeKeyword Decimal = new TypeKeyword("decimal", 10);

        public static readonly TypeKeyword Char = new TypeKeyword("char", 11);
        public static readonly TypeKeyword String = new TypeKeyword("string", 12);

        public static readonly TypeKeyword Bool = new TypeKeyword("bool", 13);

        public static readonly TypeKeyword Array = new TypeKeyword("Array", 14);
        public static readonly TypeKeyword Object = new TypeKeyword("object", 15);
    }
}
