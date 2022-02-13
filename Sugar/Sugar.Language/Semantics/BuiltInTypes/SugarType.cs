using System;
using System.Collections.Generic;

using Sugar.Language.Tokens.Enums;
using Sugar.Language.Tokens.Operators;

using Sugar.Language.Semantics.Analysis.BuiltInTypes.Enums;

namespace Sugar.Language.Semantics.BuiltInTypes
{
    internal abstract class SugarType 
    {
        public abstract bool ReferenceType { get; }

        protected TypeEnum ImplicitConversions;
        protected TypeEnum ExplicitConversions;

        protected OperatorKind[] UnaryOperators;
        protected Dictionary<OperatorKind, TypeEnum> BinaryOperators;

        public SugarType(TypeEnum _implicit, TypeEnum _explicit, OperatorKind[] unaryOperators, Dictionary<OperatorKind, TypeEnum> binaryOperators) 
        {
            ImplicitConversions = _implicit;
            ExplicitConversions = _implicit | _explicit;

            UnaryOperators = unaryOperators;
            BinaryOperators = binaryOperators;
        }

        public bool MatchExplicitConversion(TypeEnum toMatch) => (toMatch & ExplicitConversions) == toMatch;
        public bool MatchImplicitConversion(TypeEnum toMatch) => (toMatch & ImplicitConversions) == toMatch;

        public bool MatchOperator(Operator operatorToMatch, TypeEnum otherOperhand)
        {
            if (BinaryOperators == null)
                return false;

            foreach (var opSet in BinaryOperators)
                if (opSet.Key == operatorToMatch.OperatorType)
                    if((otherOperhand & opSet.Value) == otherOperhand)
                        return true;

            return false;
        }

        public bool MatchOperator(Operator operatorToMatch)
        {
            if (UnaryOperators == null)
                return false;

            foreach (var op in UnaryOperators)
                if(op == operatorToMatch.OperatorType)
                    return true;

            return false;
        }
    }
}
