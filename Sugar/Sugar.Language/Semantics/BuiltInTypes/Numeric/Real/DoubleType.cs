using System;

using Sugar.Language.Tokens.Enums;
using Sugar.Language.Tokens.Operators;

using Sugar.Language.Semantics.Analysis.BuiltInTypes.Enums;

namespace Sugar.Language.Semantics.BuiltInTypes.Numeric.Real
{
    internal sealed class DoubleType : RealType
    {
        public DoubleType() : base(TypeEnum.Double, TypeEnum.Double, TypeEnum.Float)
        {

        }
    }
}
