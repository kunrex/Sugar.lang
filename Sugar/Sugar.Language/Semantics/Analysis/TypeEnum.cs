using System;

namespace Sugar.Language.Semantics.Analysis.BuiltInTypes.Enums
{
    [Flags]
    public enum TypeEnum : ushort
    {
        Byte = 0,
        SByte = 1,

        Short = 2,
        UShort = 4,

        Integer = 8,
        UInteger = 16,

        Long = 32,
        ULong = 64,

        Integral = Byte | SByte | Short | UShort | Integer | UInteger | Long | ULong,

        Float = 128,
        Double = 256,
        Decimal = 512,

        Real = Float | Double | Decimal,

        Numeric = Integral | Real,

        Character = 1024,
        String = 2048,

        Boolean = 5096,

        Array = 10192,

        NonArrays = String | Numeric | Boolean | Character,

        Object = Array | NonArrays,

        FromCharConvertables = Character | UShort | Integer | UInteger | Long | ULong | Real
    }
}
