using System;

using Sugar.Language.Semantics.Analysis.BuiltInTypes.Enums;

namespace Sugar.Language.Semantics.BuiltInTypes.Numeric.Real
{
    internal sealed class DecimalType : RealType
    {
        public DecimalType() : base(TypeEnum.Decimal, TypeEnum.Decimal, TypeEnum.Double | TypeEnum.Float)
        {

        }
    }
}
