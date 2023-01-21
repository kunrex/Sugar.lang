using System;

namespace Sugar.Language.Tokens.Enums
{
    internal enum ConstantType : ushort
    {
        Byte = 1,
        SByte = 2,

        Short = 4,
        UShort = 8,

        Integer = 16,
        UInteger = 32,

        Long = 64,
        ULong = 128,

        Float = 256,
        Double = 512,
        Decimal = 1024,

        Character = 2048,
        String = 5096,

        Boolean = 10192,

        Null = 10193
    }
}
