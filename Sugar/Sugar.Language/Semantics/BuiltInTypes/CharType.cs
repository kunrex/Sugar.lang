using System;
using System.Collections.Generic;

using Sugar.Language.Tokens.Enums;

using Sugar.Language.Semantics.Analysis.BuiltInTypes.Enums;

namespace Sugar.Language.Semantics.BuiltInTypes
{
    internal sealed class CharType : SugarType
    {
        public override bool ReferenceType { get => false; }

        public CharType() : base
            (TypeEnum.Char | TypeEnum.FromCharConvertables,
             TypeEnum.Boolean | TypeEnum.String,
             new OperatorKind[] { OperatorKind.Increment, OperatorKind.Decrement, OperatorKind.Minus, OperatorKind.Plus },
             new Dictionary<OperatorKind, TypeEnum>
             {
                 { OperatorKind.Addition, TypeEnum.FromCharConvertables },
                 { OperatorKind.Subtraction, TypeEnum.FromCharConvertables },
                 { OperatorKind.Multiplication, TypeEnum.FromCharConvertables },
                 { OperatorKind.Division, TypeEnum.FromCharConvertables },
                 { OperatorKind.Modulus, TypeEnum.FromCharConvertables },

                 { OperatorKind.RightShit, TypeEnum.FromCharConvertables },
                 { OperatorKind.LeftShift, TypeEnum.FromCharConvertables },
                 { OperatorKind.BitwiseAnd, TypeEnum.FromCharConvertables },
                 { OperatorKind.BitwiseOr, TypeEnum.FromCharConvertables },
                 { OperatorKind.BitwiseXor, TypeEnum.FromCharConvertables }
             })
        {
            
        }

       
    }
}
