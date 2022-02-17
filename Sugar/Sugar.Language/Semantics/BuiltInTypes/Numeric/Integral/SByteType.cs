using System;

using Sugar.Language.Semantics.Analysis.BuiltInTypes.Enums;

namespace Sugar.Language.Semantics.BuiltInTypes.Numeric.Integral
{
    internal sealed class SByteType : IntegralType
    {
        public SByteType() : base
            (TypeEnum.SByte,
             TypeEnum.Short | TypeEnum.Int | TypeEnum.Long | TypeEnum.Real,
             TypeEnum.Byte | TypeEnum.UShort | TypeEnum.UInt | TypeEnum.ULong,
             TypeEnum.SByte 
            )
        {
            
        }
    }
}
