using System;
using System.Collections.Generic;

using Sugar.Language.Tokens.Enums;

using Sugar.Language.Semantics.Analysis.BuiltInTypes.Enums;

namespace Sugar.Language.Semantics.BuiltInTypes
{
    internal sealed class StringType : SugarType
    {
        public override bool ReferenceType { get => true; }

        public StringType() : base
            (TypeEnum.String | TypeEnum.Array,
             TypeEnum.Char,
             null,
             new Dictionary<OperatorKind, TypeEnum>
             {
                 { OperatorKind.Addition, TypeEnum.Object }
             })
        {
            
        }
    }
}
