﻿using System;

using Sugar.Language.Tokens.Enums;
using Sugar.Language.Tokens.Operators;

using Sugar.Language.Semantics.Analysis.BuiltInTypes.Enums;

namespace Sugar.Language.Semantics.BuiltInTypes
{
    internal sealed class CharType : SugarType
    {
        public override bool ReferenceType { get => false; }

        public CharType() : base(TypeEnum.Char | TypeEnum.FromCharConvertables, TypeEnum.Boolean | TypeEnum.String)
        {

        }

        public override (bool, TypeEnum) MatchOperator(Operator operatorToMatch)
        {
            switch(operatorToMatch.OperatorType)
            {
                case OperatorKind.Increment:
                case OperatorKind.Decrement:
                    return (true, TypeEnum.Char);
                case OperatorKind.Plus:
                case OperatorKind.Minus:
                case OperatorKind.BitwiseNot:
                    return (true, TypeEnum.Int);
                default:
                    return (false, 0);
            }
        }

        public override (bool, TypeEnum) MatchOperator(Operator operatorToMatch, TypeEnum otherOperhand)
        {
            switch (operatorToMatch.OperatorType)
            {
                case OperatorKind.Addition:
                case OperatorKind.Subtraction:
                case OperatorKind.Multiplication:
                case OperatorKind.Division:
                case OperatorKind.Modulus:
                    if(MatchType(TypeEnum.Char | TypeEnum.Integral, otherOperhand))
                        return (true, TypeEnum.Int);
                    break;
                case OperatorKind.RightShit:
                case OperatorKind.LeftShift:
                    if(MatchType(TypeEnum.Char | TypeEnum.Byte | TypeEnum.SByte | TypeEnum.Short | TypeEnum.UShort | TypeEnum.Int, otherOperhand))
                        return (true, TypeEnum.Int);
                    break;
            }

            return (false, 0);
        }
    }
}
