using System;

using Sugar.Language.Semantics.Analysis.BuiltInTypes.Enums;

namespace Sugar.Language.Tokens.Enums
{
    internal enum RealType : byte
    {
        Float = TypeEnum.Float >> 8,
        Double = TypeEnum.Double >> 8,
        Decimal = TypeEnum.Decimal >> 8
    }
}
