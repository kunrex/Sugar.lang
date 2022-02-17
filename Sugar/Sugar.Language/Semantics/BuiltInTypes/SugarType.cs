using System;

using Sugar.Language.Tokens.Operators;

using Sugar.Language.Semantics.Analysis.BuiltInTypes.Enums;

namespace Sugar.Language.Semantics.BuiltInTypes
{
    internal abstract class SugarType 
    {
        public abstract bool ReferenceType { get; }

        protected TypeEnum ImplicitConversions;
        protected TypeEnum ExplicitConversions;

        public SugarType(TypeEnum _implicit, TypeEnum _explicit)
        {
            ImplicitConversions = _implicit | TypeEnum.Object;
            ExplicitConversions = _implicit | _explicit;
        }

        public bool MatchExplicitConversion(TypeEnum toMatch) => (toMatch & ExplicitConversions) == toMatch;
        public bool MatchImplicitConversion(TypeEnum toMatch) => (toMatch & ImplicitConversions) == toMatch;

        protected bool MatchType(TypeEnum allowed, TypeEnum current) => (current & allowed) == current;

        public abstract (bool, TypeEnum) MatchOperator(Operator operatorToMatch);
        public abstract (bool, TypeEnum) MatchOperator(Operator operatorToMatch, TypeEnum otherOperhand);
    }
}
