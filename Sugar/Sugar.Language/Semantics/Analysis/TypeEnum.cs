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

        Int = 8,
        UInt = 16,

        Long = 32,
        ULong = 64,

        Integral = Byte | SByte | Short | UShort | Int | UInt | Long | ULong,

        Float = 128,
        Double = 256,
        Decimal = 512,

        Real = Float | Double | Decimal,

        Numeric = Integral | Real,

        Char = 1024,
        String = 2048,

        Boolean = 5096,

        Array = 10192,

        NonArrays = String | Numeric | Boolean | Char,

        Object = Array | NonArrays,

        FromCharConvertables = Char | UShort | Int | UInt | Long | ULong | Real
    }
}
