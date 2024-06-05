using System;
namespace Sugar.Language.Analysis.ProjectStructure.Enums
{
    [Flags]
    internal enum InternalTypeEnum : ushort
    {
        Byte = WrapperTypeEnum.Byte,
        SByte = WrapperTypeEnum.SByte,

        Short = WrapperTypeEnum.Short,
        UShort = WrapperTypeEnum.UShort,

        Integer = WrapperTypeEnum.Integer,
        UInteger = WrapperTypeEnum.UInteger,

        Long = WrapperTypeEnum.Long,
        ULong = WrapperTypeEnum.ULong,

        Integral = Byte | SByte | Short | UShort | Integer | UInteger | Long | ULong,

        Float = WrapperTypeEnum.Float,
        Double = WrapperTypeEnum.Double,
        Decimal = WrapperTypeEnum.Decimal,

        Real = Float | Double | Decimal,

        Numeric = Integral | Real,

        Character = WrapperTypeEnum.Character,
        String = WrapperTypeEnum.String,

        Boolean = WrapperTypeEnum.Boolean,

        Array = 20384,

        NonArrays = String | Numeric | Boolean | Character,

        Object = Array | NonArrays,

        FromCharConvertables = Character | UShort | Integer | UInteger | Long | ULong | Real
    }
}
