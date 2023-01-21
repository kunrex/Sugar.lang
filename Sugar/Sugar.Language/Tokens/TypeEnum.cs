using System;

using Sugar.Language.Tokens.Enums;

namespace Sugar.Language.Semantics.Analysis.BuiltInTypes.Enums
{
    [Flags]
    public enum TypeEnum : ushort
    {
        Byte = ConstantType.Byte,
        SByte = ConstantType.SByte,

        Short = ConstantType.Short,
        UShort = ConstantType.UShort,

        Integer = ConstantType.Integer,
        UInteger = ConstantType.UInteger,

        Long = ConstantType.Long,
        ULong = ConstantType.ULong,

        Integral = Byte | SByte | Short | UShort | Integer | UInteger | Long | ULong,

        Float = ConstantType.Float,
        Double = ConstantType.Double,
        Decimal = ConstantType.Decimal,

        Real = Float | Double | Decimal,

        Numeric = Integral | Real,

        Character = ConstantType.Character,
        String = ConstantType.String,

        Boolean = ConstantType.Boolean,

        Array =  20384,

        NonArrays = String | Numeric | Boolean | Character,

        Object = Array | NonArrays,

        FromCharConvertables = Character | UShort | Integer | UInteger | Long | ULong | Real
    }
}
