using System;

using Sugar.Language.Tokens.Enums;
using Sugar.Language.Tokens.Operators;

using Sugar.Language.Semantics.Analysis.BuiltInTypes.Enums;

namespace Sugar.Language.Semantics.BuiltInTypes
{
    internal sealed class BooleanType : SugarType
    {
        public override bool ReferenceType { get => false; }

        public BooleanType() : base(TypeEnum.Boolean, TypeEnum.Char | TypeEnum.String | TypeEnum.Integral)
        {

        }

        public override (bool, TypeEnum) MatchOperator(Operator operatorToMatch)
        {
            if (operatorToMatch.OperatorType == OperatorKind.Not)
                return (true, TypeEnum.Boolean);

            return (false, 0);
        }

        public override (bool, TypeEnum) MatchOperator(Operator operatorToMatch, TypeEnum otherOperhand)
        {
            switch(operatorToMatch.OperatorType)
            {
                case OperatorKind.Or:
                case OperatorKind.And:
                case OperatorKind.BitwiseOr:
                case OperatorKind.BitwiseAnd:
                case OperatorKind.BitwiseXor:
                    if (otherOperhand == TypeEnum.Boolean)
                        return (true, TypeEnum.Boolean);
                    break;
            }

            return (false, 0);
        }
    }
}
