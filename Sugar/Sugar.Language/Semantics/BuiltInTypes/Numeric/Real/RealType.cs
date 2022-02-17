using System;

using Sugar.Language.Tokens.Enums;
using Sugar.Language.Tokens.Operators;

using Sugar.Language.Semantics.Analysis.BuiltInTypes.Enums;

namespace Sugar.Language.Semantics.BuiltInTypes.Numeric.Real
{
    internal abstract class RealType : NumericType
    {
        public RealType(TypeEnum _return, TypeEnum _implicit, TypeEnum _subReal) : base(_return, _implicit, TypeEnum.Numeric, _subReal)
        {
           
        }

        public override (bool, TypeEnum) MatchOperator(Operator operatorToMatch)
        {
            switch(operatorToMatch.OperatorType)
            {
                case OperatorKind.Plus:
                case OperatorKind.Minus:
                case OperatorKind.Increment:
                case OperatorKind.Decrement:
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
                    if (MatchType(returnType | subTypes | TypeEnum.Integral, otherOperhand))
                        return (true, returnType);

                    break;
            }

            return (false, 0);
        }
    }
}
