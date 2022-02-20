using System;

using Sugar.Language.Tokens.Enums;
using Sugar.Language.Tokens.Operators;

using Sugar.Language.Semantics.Analysis.BuiltInTypes.Enums;

namespace Sugar.Language.Semantics.BuiltInTypes.Numeric.Integral
{
    internal abstract class IntegralType : NumericType
    {
        public IntegralType(TypeEnum _return, TypeEnum _implicit, TypeEnum _explicit, TypeEnum _subIngetrals) : base(_return, _implicit | TypeEnum.Char, _explicit, _subIngetrals)
        {
            
        }

        public override (bool, TypeEnum) MatchOperator(Operator operatorToMatch)
        {
            switch (operatorToMatch.OperatorType)
            {
                case OperatorKind.Plus:
                case OperatorKind.Minus:
                case OperatorKind.Increment:
                case OperatorKind.Decrement:
                case OperatorKind.BitwiseNot:
                    return (true, returnType);
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
                case OperatorKind.BitwiseAnd:
                case OperatorKind.BitwiseOr:
                case OperatorKind.BitwiseXor:
                    if (MatchType(returnType | subTypes | TypeEnum.Char, otherOperhand))
                        return (true, returnType);
                    break;
                case OperatorKind.RightShit:
                case OperatorKind.LeftShift:
                    if (MatchType(returnType | TypeEnum.Byte | TypeEnum.SByte | TypeEnum.Short | TypeEnum.UShort | TypeEnum.Int | TypeEnum.Char, otherOperhand))
                        return (true, returnType);
                    break;
            }

            return (false, 0);
        }
    }
}
