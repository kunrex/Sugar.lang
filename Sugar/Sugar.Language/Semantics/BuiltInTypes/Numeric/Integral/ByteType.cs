using System;

using Sugar.Language.Semantics.Analysis.BuiltInTypes.Enums;

namespace Sugar.Language.Semantics.BuiltInTypes.Numeric.Integral
{
    internal sealed class ByteType : IntegralType
    {
        public ByteType() : base(TypeEnum.Byte,
             TypeEnum.Short | TypeEnum.UShort | TypeEnum.Int | TypeEnum.UInt | TypeEnum.Long | TypeEnum.ULong | TypeEnum.Real,
             TypeEnum.SByte,
             TypeEnum.Byte)
        {
            
        }
    }
}
