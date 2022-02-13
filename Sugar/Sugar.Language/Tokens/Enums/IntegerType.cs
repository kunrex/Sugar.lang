using System;

using Sugar.Language.Semantics.Analysis.BuiltInTypes.Enums;

namespace Sugar.Language.Tokens.Enums
{
    internal enum IntegerType : byte
    {
        Byte = (byte)TypeEnum.Byte,
        Sbyte = (byte)TypeEnum.SByte,

        Short = (byte)TypeEnum.Short,
        UShort = (byte)TypeEnum.UShort,

        Int = (byte)TypeEnum.Int,
        UInt = (byte)TypeEnum.UInt,

        Long = (byte)TypeEnum.Long,
        ULong = (byte)TypeEnum.ULong,
    }
}
