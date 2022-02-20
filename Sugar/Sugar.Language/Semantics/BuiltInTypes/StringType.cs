using System;

using Sugar.Language.Tokens.Enums;
using Sugar.Language.Tokens.Operators;

using Sugar.Language.Semantics.Analysis.BuiltInTypes.Enums;

namespace Sugar.Language.Semantics.BuiltInTypes
{
    internal sealed class StringType : SugarType
    {
        public override bool ReferenceType { get => true; }

        public StringType() : base(TypeEnum.String | TypeEnum.Array,  TypeEnum.Char)
        {
            
        }

        public override (bool, TypeEnum) MatchOperator(Operator operatorToMatch) => (false, 0);

        public override (bool, TypeEnum) MatchOperator(Operator operatorToMatch, TypeEnum otherOperhand)
        {
            if (operatorToMatch.OperatorType == OperatorKind.Addition)
                return (true, TypeEnum.String);

            return (false, 0);
        }
    }
}
